﻿using UnityEngine;

namespace Core
{
    public class Circle
    {
        [SerializeField]
        private float xAxis;

        [SerializeField]
        private float yAxis;

        [SerializeField]
        private int steps;

        public float X
        {
            get => xAxis;
            set => xAxis = value;
        }

        public float Y
        {
            get => yAxis;
            set => yAxis = value;
        }

        public int Steps
        {
            get => steps;
            set => steps = value;
        }

        public Circle(float radius)
        {
            this.xAxis = radius;
            this.yAxis = radius;
            this.steps = 1;
        }

        public Circle(float radius, int steps)
        {
            this.xAxis = radius;
            this.yAxis = radius;
            this.steps = steps;
        }

        public Circle(float xAxis, float yAxis)
        {
            this.xAxis = xAxis;
            this.yAxis = yAxis;
            this.steps = 10;
        }

        public Circle(float xAxis, float yAxis, int steps)
        {
            this.xAxis = xAxis;
            this.yAxis = yAxis;
            this.steps = steps;
        }

        public Vector2 Evaluate(float t)
        {
            float increments = 360f / steps;
            float angle = Mathf.Deg2Rad * increments * t;
            float x = Mathf.Sin(angle) * xAxis;
            float y = Mathf.Cos(angle) * yAxis;
            return new Vector2(x, y);
        }

        public void Evaluate(float t, out Vector2 eval)
        {
            float increments = 360f / steps;
            float angle = Mathf.Deg2Rad * increments * t;
            eval.x = Mathf.Sin(angle) * xAxis;
            eval.y = Mathf.Cos(angle) * yAxis;
        }
    }
}