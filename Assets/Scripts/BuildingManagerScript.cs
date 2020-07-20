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
        ghostObject.AddComponent<BuildableObject>();
        ghostSprite = ghostObject.AddComponent<SpriteRenderer>();

        //fluxEngine
        if(fluxEnginePrefab!=null)
            BuiltEngine = BuildAtGridPosition(fluxEnginePrefab, new Vector2(Random.Range(1,5),0));
        
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

            var mpos = Camera.main.ScreenToWorldPoint(InputController.MousePosition());
            BuildAtMousePosition(buildableObjects[currentBuildableObject], mpos);

        } else {
            ghostSprite.sprite = null;
        }
    }

    private GameObject BuildAtMousePosition(GameObject prefab, Vector3 mousePos){
        objectToBuild =  prefab;
        ghostSprite.sprite = objectToBuild.GetComponent<SpriteRenderer>().sprite;
        Bounds bounds = objectToBuild.GetComponent<SpriteRenderer>().bounds;

        //Calculate Position
        var gridOrigin = vehicleCorner.position;
        var buildPos = mousePos-gridOrigin; //Take offset into account

        var xcell = Mathf.FloorToInt(buildPos.x/gridSize)*gridSize;
        var ycell = Mathf.FloorToInt(buildPos.y/gridSize)*gridSize;

        var xPos = xcell+gridOrigin.x+bounds.size.x/2;
        var yPos = ycell+gridOrigin.y+bounds.size.y/2;
        ghostObject.transform.position = new Vector3(xPos, yPos, -8);

        // CheckForSiblings();
        ghostSprite.color = CheckForSpace() ? new Color(1,0.5f,0.5f,0.8f) : new Color(0.5f,1,0.5f,1);

        //Create Object
        if(InputController.CreateObject(ICActions.keyDown) && CheckForSiblings() && !CheckForSpace()){
            print($"CELL[:{xcell},{ycell}]" );
            var obj = Instantiate(objectToBuild);
            var pos = ghostObject.transform.position;
            obj.transform.position = new Vector3(pos.x, pos.y, pos.z+1);
            obj.transform.parent = buildState.transform;
            return obj.GetComponent<BuildableObject>().Build();;
        } else {
            return null;
        }
    } 

    private GameObject BuildAtGridPosition(GameObject prefab, Vector2 gridPos){
        objectToBuild =  prefab;
        ghostSprite.sprite = objectToBuild.GetComponent<SpriteRenderer>().sprite;
        Bounds bounds = objectToBuild.GetComponent<SpriteRenderer>().bounds;

        //Calculate Position
        var gridOrigin = vehicleCorner.position;

        var xPos = gridPos.x+gridOrigin.x+bounds.size.x/2;
        var yPos = gridPos.y+gridOrigin.y+bounds.size.y/2;
        ghostObject.transform.position = new Vector3(xPos, yPos, -8);

        // CheckForSiblings();
        ghostSprite.color = CheckForSpace() ? new Color(1,0.5f,0.5f,0.8f) : new Color(0.5f,1,0.5f,1);

        //Create Object
        if(CheckForSiblings() && !CheckForSpace()){
            print($"CELL[:{gridPos.x},{gridPos.y}]" );
            var obj = Instantiate(objectToBuild);
            var pos = ghostObject.transform.position;
            obj.transform.position = new Vector3(pos.x, pos.y, pos.z+1);
            obj.transform.parent = buildState.transform;
            return obj.GetComponent<BuildableObject>().Build();
        } else {
            return null;
        }
    }

    private bool CheckForSpace(){
        //bool free = false;
        var origin = ghostObject.transform.position;
        var bounds = ghostObject.GetComponent<SpriteRenderer>().bounds.size;
        var bOffset = 0.1f;
        var size = new Vector2(bounds.x - bOffset, bounds.y - bOffset);

        RaycastHit2D box = Physics2D.BoxCast(origin, size, 0, Vector2.zero);
        return box.collider;
    }

    private bool CheckForSiblings(){
        bool left, right, top, bottom = false;
        var origin = ghostObject.transform.position;
        var requirements = objectToBuild.GetComponent<BuildableObject>().requirements;
        var distance = gridSize;
        var duration = 0.65f;

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
        if(!left && requirements.Left) return false;
        if(!right && requirements.Right) return false;
        if(!top && requirements.Top) return false;
        if(!bottom && requirements.Bottom) return false;

        return left || right || top || bottom;
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
