using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidationResults{
    public bool? restrictionsPassed = null;
    public bool? constraintsPassed = null;
    public bool? nextToBuildableSurface = null;
    public int validationsRan = 0;
    public int validationsPassed = 0;
    public bool IsInvalid{
        get => this.constraintsPassed == null || this.restrictionsPassed == null || this.nextToBuildableSurface == null;
    }

    public bool PassedAllChecks(){
        // if(validationsRan == 0) throw new System.Exception("0 Validations were run!");
        return this.validationsRan == this.validationsPassed;
    }

    public override string ToString()
    {
        return $"RestrictionsPassed = {this.restrictionsPassed}\n" + 
        $"ConstraintsPassed = {this.constraintsPassed}\n" +
        $"NextToBuildable={this.nextToBuildableSurface}\n" +
        $"Ran = {this.validationsRan}\nPassed = {this.validationsPassed}";
    }

    
}

