using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SimulationInput : MonoBehaviour
{
    public Simulation sim;

    public TMP_InputField rocketRadiusInput;
    public TMP_Dropdown rocketRadiusDropdown;

    public TMP_InputField rocketVolumeInput;
    public TMP_Dropdown rocketVolumeDropdown;

    public TMP_InputField fuelVolmeInput;
    public TMP_Dropdown fuelVolumeDropdown;

    public TMP_InputField fuelDensityInput;
    public TMP_Dropdown fuelDensityDropdown;

    public TMP_InputField tankPressureInput;
    public TMP_Dropdown tankPressureDropdown;

    public TMP_InputField nozzleRadiusInput;
    public TMP_Dropdown nozzleRadiusDropdown;

    public TMP_InputField dragCoefficientInput;

    public TMP_InputField dryMassInput;
    public TMP_Dropdown dryMassDropdown;

    public TMP_InputField adiabaticInput;

    public TMP_InputField startingHeightInput;
    public TMP_Dropdown startingHeightDropdown;

    public TMP_InputField gravityConstantInput;

    public TMP_InputField externalAirDensityInput;
    public TMP_Dropdown externalAirDensityDropdown;

    public TMP_InputField externalAirTemperatureInput;
    public TMP_Dropdown externalAirTemperatureDropdown;

    public TextMeshProUGUI errorText;

    public void Simulate()
    {
        try
        {
            sim.Initialize(GetTimeinSeconds(), GetTankVolumeInMeter3(), GetRadiusInMeters(), GetFuelVolumeInMeter3(), GetFuelDensityInKgM3(), GetTankPressureInPa(), GetNozzleRadiusInM(), GetGravityConstant(), GetDragCoefficient(), GetDryMassInKG(), GetAdiabaticIndex(), GetStartingHeightInM(), GetExternalAirDensityInKgM3(), GetAirTemperatureinK());
        }
        catch(Exception e)
        {
            errorText.text = e.Message;
        }
    }

    float GetTimeinSeconds()
    {
        return 0.00001f;
    }

    float GetRadiusInMeters()
    {
        //error handling
        if (string.IsNullOrEmpty(rocketRadiusInput.text))
        {
            throw new Exception("Rocket radius field is empty");
        }
        float val;
        if (!float.TryParse(rocketRadiusInput.text, out val))
        {
            throw new Exception("Rocket radius is not a valid number!");
        }
        if (val <= 0)
        {
            throw new Exception("Rocket radius must be a value greater than 0");
        }

        return DistanceConversionToM(val, rocketRadiusDropdown.value);
    }

    float GetTankVolumeInMeter3()
    {
        //error handling
        if (string.IsNullOrEmpty(rocketVolumeInput.text))
        {
            throw new Exception("Tank Volume field is empty");
        }
        float val;
        if (!float.TryParse(rocketVolumeInput.text, out val))
        {
            throw new Exception("Tank Volume is not a valid number!");
        }
        if (val <= 0)
        {
            throw new Exception("Tank Volume must be a value greater than 0");
        }

        return VolumeConversionToM3(val, rocketVolumeDropdown.value);
    }

    float GetFuelVolumeInMeter3()
    {
        //error handling
        if (string.IsNullOrEmpty(fuelVolmeInput.text))
        {
            throw new Exception("Fuel Volume field is empty");
        }
        float val;
        if (!float.TryParse(fuelVolmeInput.text, out val))
        {
            throw new Exception("Fuel Volume is not a valid number!");
        }
        if (val <= 0)
        {
            throw new Exception("Fuel Volume must be a value greater than 0");
        }

        return VolumeConversionToM3(val, fuelVolumeDropdown.value);
    }

    float GetFuelDensityInKgM3()
    {
        //error handling
        if (string.IsNullOrEmpty(fuelDensityInput.text))
        {
            throw new Exception("Fuel Density field is empty");
        }
        float val;
        if (!float.TryParse(fuelDensityInput.text, out val))
        {
            throw new Exception("Fuel Density is not a valid number!");
        }
        if (val <= 0)
        {
            throw new Exception("Fuel Density must be a value greater than 0");
        }

        return DensityConversionToKgM3(val, fuelDensityDropdown.value);
    }

    float GetTankPressureInPa()
    {
        //error handling
        if (string.IsNullOrEmpty(tankPressureInput.text))
        {
            throw new Exception("Tank Pressure field is empty");
        }
        float val;
        if (!float.TryParse(tankPressureInput.text, out val))
        {
            throw new Exception("Tank Pressure is not a valid number!");
        }
        if (val <= 0)
        {
            throw new Exception("Tank Pressure must be a value greater than 0");
        }

        return PressureConversionToPa(val, tankPressureDropdown.value);
    }

    float GetNozzleRadiusInM()
    {
        //error handling
        if (string.IsNullOrEmpty(nozzleRadiusInput.text))
        {
            throw new Exception("Nozzle radius field is empty");
        }
        float val;
        if (!float.TryParse(nozzleRadiusInput.text, out val))
        {
            throw new Exception("Nozzle radius is not a valid number!");
        }
        if (val <= 0)
        {
            throw new Exception("Nozzle radius must be a value greater than 0");
        }

        return DistanceConversionToM(val, nozzleRadiusDropdown.value);
    }

    float GetDragCoefficient()
    {
        if (string.IsNullOrEmpty(dragCoefficientInput.text))
        {
            throw new Exception("Drag Coefficient field is empty");
        }
        float val;
        if (!float.TryParse(dragCoefficientInput.text, out val))
        {
            throw new Exception("Drag Coefficient is not a valid number!");
        }
        if (val <= 0)
        {
            throw new Exception("Drag Coefficient must be a value greater than 0");
        }
        return val;
    }

    float GetDryMassInKG()
    {
        //error handling
        if (string.IsNullOrEmpty(dryMassInput.text))
        {
            throw new Exception("Dry Mass field is empty");
        }
        float val;
        if (!float.TryParse(dryMassInput.text, out val))
        {
            throw new Exception("Dry Mass is not a valid number!");
        }
        if (val <= 0)
        {
            throw new Exception("Dry Mass must be a value greater than 0");
        }

        return MassConversionToKG(val, dryMassDropdown.value);
    }

    float GetAdiabaticIndex()
    {
        if (string.IsNullOrEmpty(adiabaticInput.text))
        {
            throw new Exception("Adiabatic Constant field is empty");
        }
        float val;
        if (!float.TryParse(adiabaticInput.text, out val))
        {
            throw new Exception("Adiabatic Constant is not a valid number!");
        }
        if (val <= 0)
        {
            throw new Exception("Adiabatic Constant must be a value greater than 0");
        }
        return val;
    }

    float GetStartingHeightInM()
    {
        //error handling
        if (string.IsNullOrEmpty(startingHeightInput.text))
        {
            throw new Exception("Starting Height field is empty");
        }
        float val;
        if (!float.TryParse(startingHeightInput.text, out val))
        {
            throw new Exception("Starting Height is not a valid number!");
        }
        if (val <= 0)
        {
            throw new Exception("Starting Height must be a value greater than 0");
        }

        return DistanceConversionToM(val, startingHeightDropdown.value);
    }

    float GetGravityConstant()
    {
        //error handling
        if (string.IsNullOrEmpty(gravityConstantInput.text))
        {
            throw new Exception("Gravity Constant field is empty");
        }
        float val;
        if (!float.TryParse(gravityConstantInput.text, out val))
        {
            throw new Exception("Gravity Constant is not a valid number!");
        }
        if (val <= 0)
        {
            throw new Exception("Gravity Constant must be a value greater than 0");
        }

        return val;
    }

    float GetExternalAirDensityInKgM3()
    {
        //error handling
        if (string.IsNullOrEmpty(externalAirDensityInput.text))
        {
            throw new Exception("Fuel Density field is empty");
        }
        float val;
        if (!float.TryParse(externalAirDensityInput.text, out val))
        {
            throw new Exception("Fuel Density is not a valid number!");
        }
        if (val <= 0)
        {
            throw new Exception("Fuel Density must be a value greater than 0");
        }

        return DensityConversionToKgM3(val, externalAirDensityDropdown.value);
    }

    float GetAirTemperatureinK()
    {
        //error handling
        if (string.IsNullOrEmpty(externalAirTemperatureInput.text))
        {
            throw new Exception("External Air Temperature field is empty");
        }
        float val;
        if (!float.TryParse(externalAirTemperatureInput.text, out val))
        {
            throw new Exception("External Air Temperature is not a valid number!");
        }
        if (val <= 0)
        {
            throw new Exception("External Air Temperature must be a value greater than 0");
        }

        return TemperatureConversionToKelvin(val, rocketRadiusDropdown.value);
    }

    float DistanceConversionToM(float val, int dropdownValue)
    {
        switch (dropdownValue)
        {
            case 0: //cm
                return val / 100;
            case 1: //m
            default:
                return val;
            case 2: //mm
                return val / 1000;
            case 3: //in
                return val * 0.0254f;
            case 4: //ft
                return val * 0.3048f;
        }
    }

    float VolumeConversionToM3(float val, int dropdownValue)
    {
        switch (dropdownValue)
        {
            case 0: //L
                return val * 0.001f;
            case 1: //ml
                return val * 0.000001f;
            case 2: //m3
            default:
                return val;
            case 3: //ounces
                return (val * 0.0295735f) * 0.001f;

        }
    }

    float DensityConversionToKgM3(float val, int dropdownValue)
    {
        switch (dropdownValue)
        {
            case 0: //kg/m3
            default:
                return val;
            case 1: //kg/L
                return val * 1000;

        }
    }

    float MassConversionToKG(float val, int dropdownValue)
    {
        switch (dropdownValue)
        {
            case 0: //g
                return val / 1000;
            case 1: //kg
            default:
                return val;
            case 2: //lbs
                return val * 0.453592f;

        }
    }

    float TemperatureConversionToKelvin(float val, int dropdownValue)
    {
        switch (dropdownValue)
        {
            case 0: //C
                return val + 273;
            case 1: //F
                return ((val - 32) / 1.8f) + 273;
            case 2: //Kelvin
            default:
                return val;
        }
    }

    float PressureConversionToPa(float val, int dropdownValue)
    {
        switch (dropdownValue)
        {
            case 0: //PSI
                return val * 6894.76f;
            case 1: //atm
                return val * 101325;
            case 2: //bar
                return val * 100000;
            case 3: //Pa
            default:
                return val;
            case 4: //kPa
                return val * 1000;

        }
    }

}
