using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManagerScript : MonoBehaviour
{

    //Objects
    public GameObject VehicleObject;
    public GameObject buildState;
    Transform vehicleCorner;

    //Flux Engine
    public GameObject fluxEnginePrefab;
    public GameObject BuiltEngine;

    //Settings
    public bool BuildMode = false;
    public int gridSize = 16;

    //BuildMode
    public List<GameObject> buildableObjects; 
    [SerializeField]
    private int currentBuildableObject = 0;
    public int CurrentBuildableObject { 
        get => currentBuildableObject; 
        set{
            if( !(value+1 > buildableObjects.Count || value < 0) ){
                currentBuildableObject = value;
            }
        } 
    }

    private GameObject objectToBuild;
    public GameObject ghostObject;
    private SpriteRenderer ghostSprite;
    public LayerMask buildingMask;
    

    void Start(){
        Begin();
    }

    void Begin(){
        if(VehicleObject == null){
            throw new System.Exception("VehicleObject cannot be NULL");
        }
        //Get vehicle corner
        vehicleCorner = new GameObject("TopLeftCorner").transform;
        var vBounds = VehicleObject.GetComponent<BoxCollider2D>().bounds;
        vehicleCorner.position = new Vector2(vBounds.min.x, vBounds.max.y);
        vehicleCorner.parent = VehicleObject.transform;
        buildState = new GameObject("Built");
        buildState.transform.parent = VehicleObject.transform;
        

        //Building Ghost
        ghostObject = new GameObject("BuildingObject");
        ghostObject.AddComponent<BuildableObject>().isGhost = true;
        ghostObject.AddComponent<BoxCollider2D>().isTrigger = true;
        ghostSprite = ghostObject.AddComponent<SpriteRenderer>();

        //fluxEngine
        // if(fluxEnginePrefab!=null)
        //     BuiltEngine = PreviewAtGridPosition(fluxEnginePrefab, new Vector2(Random.Range(1,5),0));
        
    }

    void Update(){
        //temporal "vehicleDestroyed" gameover
        if(BuiltEngine!=null)
            if(BuiltEngine.TryGetComponent<Health>(out var comp))
                if(!comp.IsAlive)
                    GameHelper.Player.gameObject.GetComponent<Health>().killEntity();

        if(InputController.Build(ICActions.keyDown)){
            BuildMode = !BuildMode;
        }

        if(BuildMode){
            //Change Selected Item
            if(InputController.LeftArrow(ICActions.keyDown) || InputController.MouseScroll() > 0){
                CurrentBuildableObject = CurrentBuildableObject-1;
            } else if(InputController.RightArrow(ICActions.keyDown) || InputController.MouseScroll() < 0){
                CurrentBuildableObject = CurrentBuildableObject+1;
            }

            var mpos = MouseToGridPosition( Camera.main.ScreenToWorldPoint(InputController.MousePosition()) );
            var toBeBuilt = buildableObjects[currentBuildableObject];
            var canBeBuilt = PreviewAtGridPosition(toBeBuilt, mpos );
            if(InputController.CreateObject(ICActions.keyDown) && canBeBuilt)
                Build(toBeBuilt, mpos);
        
        } else {
            ghostSprite.sprite = null;
        }
    }

    private GameObject Build(GameObject objectToBuild, Vector2 posInGrid){
        print($"CELL[:{posInGrid.x},{posInGrid.y}]" );
        var obj = Instantiate(objectToBuild);
        var pos = ghostObject.transform.position;
        obj.transform.position = new Vector3(pos.x, pos.y, pos.z+1);
        obj.transform.parent = buildState.transform;
        return obj.GetComponent<BuildableObject>().Build();
    }

    private bool PreviewAtGridPosition(GameObject prefab, Vector2 gridPos){
        objectToBuild = prefab;
        ghostSprite.sprite = objectToBuild.GetComponent<SpriteRenderer>().sprite;
        Bounds bounds = objectToBuild.GetComponent<SpriteRenderer>().bounds;

        //Calculate Position
        var gridOrigin = vehicleCorner.position;

        var xPos = gridPos.x+gridOrigin.x+bounds.size.x/2;
        var yPos = gridPos.y+gridOrigin.y+bounds.size.y/2;
        ghostObject.transform.position = new Vector3(xPos, yPos, -8);

        var canBeBuilt = CanBeBuilt(objectToBuild.GetComponent<BoxCollider2D>().size);

        // CheckForSiblings();
        ghostSprite.color = !canBeBuilt ? new Color(1,0.5f,0.5f,0.8f) : new Color(0.5f,1,0.5f,1);

        return canBeBuilt;
    }

    private bool CanBeBuilt(Vector2 size){
        return FollowsRules(size) && !SpaceIsOccupied();
    }

    private bool SpaceIsOccupied(){
        // print(objectToBuilt.GetComponent<BoxCollider2D>().bounds);
        //bool free = false;
        var origin = ghostObject.transform.position;
        var bounds = objectToBuild.GetComponent<BoxCollider2D>().size;
        var bOffset = 0.1f;
        var size = new Vector2(bounds.x - bOffset, bounds.y - bOffset);

        RaycastHit2D box = Physics2D.BoxCast(origin, size, 0, Vector2.zero, 1, buildingMask );
        DebugTools.DrawBounds( new Bounds(origin, size), Color.yellow );
        if(box) DebugTools.DrawBounds(box.collider.bounds, Color.red);
        return box.collider;
    }

    private bool FollowsRules(Vector2 size){
        var overallValidations = new ValidationResults();
        for(int i = 0; i < size.x; i++){
            for(int j = 0; j < size.y; j++){
                var origin = ghostObject.transform.position;
                var x = origin.x - size.x/2;
                var y = origin.y - size.y/2;
                // if( CheckAdjacentCells(new Vector2(x + 0.5f + (1*i), y + 0.5f + (1 * j)))){
                //     passed++;
                // }
                var val = CheckAdjacentCells(new Vector2(x + 0.5f + (1*i), y + 0.5f + (1 * j)));
                overallValidations.validationsRan++;
                if(!val.IsInvalid){
                    if(!val.constraintsPassed.Value){
                        return false;
                    }

                    if(val.restrictionsPassed.Value && val.nextToBuildableSurface.Value){
                        overallValidations.validationsPassed++;
                    }
                }
                
            }
        }
        // print(overallValidations);
        if(overallValidations.validationsPassed > 0) 
            return true;
        else
            return false;
    }

    private ValidationResults CheckAdjacentCells(Vector2 origin){

        var validation = new ValidationResults();

        bool left, right, top, bottom = false;
        // var origin = ghostObject.transform.position;
        var obj = objectToBuild.GetComponent<BuildableObject>();
        var requirements = obj.requirements;
        var constraints = obj.constraints;
        var distance = gridSize;
        var duration = 0;

        //Left cast
        RaycastHit2D leftRay = Physics2D.Raycast(origin, Vector2.left, distance, buildingMask);
        Debug.DrawRay(origin, Vector2.left * distance, leftRay.collider==null?Color.red:Color.green, duration);
        left = leftRay.collider;

        //Right cast
        RaycastHit2D rightRay = Physics2D.Raycast(origin, Vector2.right, distance, buildingMask);
        Debug.DrawRay(origin, Vector2.right * distance, rightRay.collider==null?Color.red:Color.green, duration);
        right = rightRay.collider;

        //Top cast
        RaycastHit2D topRay = Physics2D.Raycast(origin, Vector2.up, distance, buildingMask);
        Debug.DrawRay(origin, Vector2.up * distance, topRay.collider==null?Color.red:Color.green, duration);
        top = topRay.collider;

        //Bottom cast
        RaycastHit2D bottomRay = Physics2D.Raycast(origin, Vector2.down, distance, buildingMask);
        Debug.DrawRay(origin, Vector2.down * distance, bottomRay.collider==null?Color.red:Color.green, duration);
        bottom = bottomRay.collider;

        //Check Requirements
        if(!left && requirements.Left) validation.restrictionsPassed = false;
        if(!right && requirements.Right) validation.restrictionsPassed = false;
        if(!top && requirements.Top) validation.restrictionsPassed = false;
        if(!bottom && requirements.Bottom) validation.restrictionsPassed = false;
        if(!(left || right) && requirements.AnySide) validation.restrictionsPassed = false;
        if(validation.restrictionsPassed != false)
            validation.restrictionsPassed = true;

        //Check Constraints
        if(left && constraints.Left) validation.constraintsPassed = false;
        if(right && constraints.Right) validation.constraintsPassed = false;
        if(top && constraints.Top) validation.constraintsPassed = false;
        if(bottom && constraints.Bottom) validation.constraintsPassed = false;
        if((left || right) && constraints.AnySide) validation.constraintsPassed = false;
        if(validation.constraintsPassed != false)
            validation.constraintsPassed = true;

        validation.nextToBuildableSurface = left || right || top || bottom;
        return validation;
    }

    private Vector2 MouseToGridPosition(Vector3 mousePos){
        //Calculate Position
        var gridOrigin = vehicleCorner.position;
        var buildPos = mousePos-gridOrigin; //Take offset into account

        var xcell = Mathf.FloorToInt(buildPos.x/gridSize)*gridSize;
        var ycell = Mathf.FloorToInt(buildPos.y/gridSize)*gridSize;
        return new Vector2(xcell, ycell);
    }

    private void OnDrawGizmos() {
        #if DEBUG
            if(BuildMode) DrawGizmoGrid();
        #endif
    }

    void DrawGizmoGrid(){
        if(vehicleCorner == null){
            return;
        }
        var gridOrigin = vehicleCorner.position;
        Gizmos.color = new Color(1,1,1,0.2f);
        //Horizontal lines
        for(int i = 0; i < 7; i++){
            var or = gridOrigin + new Vector3(0, i*gridSize, 0);
            Gizmos.DrawLine(or+new Vector3(-4,0,0), or+(new Vector3(16,0,0)) );
        }

        //Vertical lines
        for(int i = -2; i < 9; i++){
            var or = gridOrigin + new Vector3(i*gridSize, 0, 0);
            Gizmos.DrawLine(or, or+(new Vector3(0,12,0)) );
        }
    }
}
