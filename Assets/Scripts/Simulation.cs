using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Simulation : MonoBehaviour
{
    const float UNIVERSAL_GAS_CONSTANT = 8.31432f;
    const float MOLAR_AIR_MASS = 0.0289644f;

    //starting conditions
    public float timeStepSeconds;
    public float rocketVolumeM3;
    public float rocketRadiusM;
    public float fuelVolumeM3;
    public float fuelDensityKgM3;
    public float pressurePa;
    public float nozzleRadiusM;
    public float gravityMS2;
    public float dragCoefficient;
    public float dryMassKg;
    public float adiabaticIndexConstant;
    public float startingHeightSeaLevelM;
    public float externalAirDensityKgM3;
    public float airTemperature;
    

    //derived values
    float wetMass;
    float dragForce;

    //live conditions
    float rTime;
    float rCurrentMass;
    float rCurrentInternalPressure;
    float rCurrentMassLossRate;
    float rCurrentFuelVolume;
   // float rCurrentForce;
    float rCurrentVelocity;
    float rCurrentHeight;
    float rCurrentExternalAirPressure;

    //Timesteps
    List<Timestep> timeSteps;
    public TimestepUI timestepUI;

    //Results
    float maxHeight;
    float maxSpeed;
    float flightTime;
    float fuelBurnout;

    public TextMeshProUGUI flightTimeText;
    public TextMeshProUGUI maxSpeedText;
    public TextMeshProUGUI maxHeightText;
    public TextMeshProUGUI fuelBurnoutTimeText;


    public void Initialize(float timestep, float rocketVolume, float rocketRadius, float fuelVolume, float fuelDensity, float pressure, float nozzleRadius, float gravity, float dragConstant, float dryMass, float adiabatic, float startingHeight, float externalAirDensity, float airTemp)
    {
        timeSteps = new List<Timestep>();
        //set all values
        timeStepSeconds = timestep;
        rocketVolumeM3 = rocketVolume;
        rocketRadiusM = rocketRadius;
        fuelVolumeM3 = fuelVolume;
        fuelDensityKgM3 = fuelDensity;
        pressurePa = pressure;
        nozzleRadiusM = nozzleRadius;
        gravityMS2 = gravity;
        dragCoefficient = dragConstant;
        dryMassKg = dryMass;
        adiabaticIndexConstant = adiabatic;
        startingHeightSeaLevelM = startingHeight;
        externalAirDensityKgM3 = externalAirDensity;
        airTemperature = airTemp;

        //starting conditions
        CalculateStartingConditions();

        Debug.Log("entered values:");
        Debug.Log("Timestep: " + timeStepSeconds);
        Debug.Log("Rocket Volume : " + rocketVolumeM3);
        Debug.Log("Radius: " + rocketRadiusM);
        Debug.Log("Fuel Vol: " + fuelVolumeM3);
        Debug.Log("Fuel Den: " + fuelDensityKgM3);
        Debug.Log("Pressure: " + pressurePa);
        Debug.Log("Noz: " + nozzleRadiusM);
        Debug.Log("Grav: " + gravityMS2);
        Debug.Log("Drag: " + dragCoefficient);
        Debug.Log("dry: " + dryMassKg);
        Debug.Log("adiabatic: " + adiabaticIndexConstant);
        Debug.Log("height: " + startingHeightSeaLevelM);
        Debug.Log("air density: " + externalAirDensityKgM3);
        Debug.Log("temp: " + airTemperature);
        Debug.Log("wet mass: " + wetMass);
        Debug.Log("drag: " + dragForce);
        Debug.Log("--------");

        fuelBurnout = -1;
        maxHeight = -1;
        maxSpeed = -1;
        flightTime = -1;

        do //Run the simulation until the rocket makes it back to the ground
        {
            CalculateNewParameters();
            CalculateMilestones();
            timeSteps.Add(new Timestep(rTime, rCurrentVelocity, rCurrentHeight, rCurrentMass, rCurrentMass - dryMassKg));
           // Debug.Log(string.Format("Time: {0}\nSpeed: {1}\nHeight: {2}\nMass: {3}\nRemaining Fuel: {4}", rTime, rCurrentVelocity, rCurrentHeight, rCurrentMass, (rCurrentMass - dryMassKg)));
        }
        while (rCurrentHeight > 0);
        //timestepUI.RenderTimesteps(timeSteps);

        ShowData();
    }

    void CalculateStartingConditions()
    {
        rTime = 0;
        wetMass = dryMassKg + (fuelVolumeM3 * fuelDensityKgM3); //assume that air is weightless for now
        dragForce = 0.5f * dragCoefficient * externalAirDensityKgM3 * (Mathf.Pow(rocketRadiusM, 2) * Mathf.PI);

        rCurrentInternalPressure = pressurePa;
        rCurrentVelocity = 0;
        rCurrentMass = wetMass;
        rCurrentFuelVolume = fuelVolumeM3;
        rCurrentHeight = 0;
    }

    void CalculateMilestones()
    {
        flightTime = rTime;
        if(fuelBurnout == -1)
        {
            if(rCurrentFuelVolume == 0)
            {
                fuelBurnout = rTime;
            }
        }
        if(rCurrentVelocity > maxSpeed)
        {
            maxSpeed = rCurrentVelocity;
        }
        if(rCurrentHeight > maxHeight)
        {
            maxHeight = rCurrentHeight;
        }
    }

    void ShowData()
    {
        maxHeightText.text = maxHeight.ToString("F2") + "m";
        maxSpeedText.text = maxSpeed.ToString("F2") + "m/s";
        flightTimeText.text = flightTime.ToString("F2") + "s";
        fuelBurnoutTimeText.text = fuelBurnout.ToString("F3") + "s";
    }

    void CalculateNewParameters()
    {
        //advance time
        rTime += timeStepSeconds;

        //Estimate the starting pressure using bernoulli's principle
        float externalAirPressurePa = 101325 * Mathf.Exp(-gravityMS2 * MOLAR_AIR_MASS * (rCurrentHeight + startingHeightSeaLevelM) / (UNIVERSAL_GAS_CONSTANT * airTemperature));

        //initialize variables
        float changeInVelocity = 0;
        float exhaustVelocity = Mathf.Sqrt(2 * (rCurrentInternalPressure - externalAirPressurePa) / fuelDensityKgM3);

        rCurrentMassLossRate = Mathf.PI * Mathf.Pow(nozzleRadiusM, 2) * fuelDensityKgM3 * exhaustVelocity; //dM/dt
        rCurrentMass = rCurrentMass - (rCurrentMassLossRate * timeStepSeconds); //Reduce mass by the current change in mass over the provided timescale

        //mass can't go below the dry mass of the rocket
        if(rCurrentMass < dryMassKg)
        {
            rCurrentMass = dryMassKg;
        }
        //Calculate the current volume of fuel remaining
        rCurrentFuelVolume = (rCurrentMass - dryMassKg) / fuelDensityKgM3;

        //Calculate the new internal pressure
        rCurrentInternalPressure = pressurePa * Mathf.Pow((rocketVolumeM3 + ((wetMass - rCurrentMass) / fuelDensityKgM3)) / rocketVolumeM3, -adiabaticIndexConstant);

        //thrust if accelerating
        if (rCurrentMass > dryMassKg)
        {
            float rThrust = 2 * Mathf.PI * nozzleRadiusM * nozzleRadiusM * (rCurrentInternalPressure - externalAirPressurePa); //acceleration
            float rChangeinVelocity = (rThrust / rCurrentMass) - gravityMS2 - ((dragForce / rCurrentMass) * rCurrentVelocity * rCurrentVelocity);
            rCurrentVelocity = rCurrentVelocity + (rChangeinVelocity * timeStepSeconds);
        }
        else //thrust under gravity
        {
            changeInVelocity = -gravityMS2 - (dragForce / rCurrentMass * rCurrentVelocity * rCurrentVelocity);
            rCurrentVelocity += changeInVelocity * timeStepSeconds;
        }

        //update the current height
        rCurrentHeight += rCurrentVelocity * timeStepSeconds;
    }

}
