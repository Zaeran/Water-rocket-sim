using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timestep
{
    float _time;
    float _speed;
    float _height;
    float _mass;
    float _remainingFuel;

    public Timestep(float time, float speed, float height, float mass, float remainingFuel)
    {
        _time = time;
        _speed = speed;
        _height = height;
        _mass = mass;
        _remainingFuel = remainingFuel;
    }

    public float Time
    {
        get { return _time; }
    }

    public float Speed
    {
        get { return _speed; }
    }

    public float Height
    {
        get { return _height; }
    }

    public float Mass
    {
        get { return _mass; }
    }

    public float RemainingFuel
    {
        get { return _remainingFuel; }
    }
}
