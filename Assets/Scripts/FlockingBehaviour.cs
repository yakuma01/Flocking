using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingBehaviour : MonoBehaviour
{
    public List<Vehicle> boids;
    //public int numberOfBoids = 300;
    //public GameObject boidPrefab;

    [Range(0,5)] 
    public float separationForce = 1f;
    [Range(0, 5)] 
    public float cohesionForce = 1f;
    [Range(-5, 5)] 
    public float alignmentForce = 1f;
    [Range(-5, 10)]
    public float homeForce = 1f;
    public float flockRadius = 10;
    public float neighbourhoodRadius = 10;

    public float maxSpeed = 40;
    public float minSpeed = 20;
    public float maxSteering = 5;

    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < numberOfBoids; i++)
        //{
        //    GameObject boid = Instantiate(boidPrefab);
        //    boids.Add(GetComponent<Vehicle>());
        //}
        foreach (Vehicle boid in boids)
        {
            boid.velocity = Random.onUnitSphere * boid.maxSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var boid in boids)
        {
            AdjustBoidParameters(boid);

            List<Vehicle> neighbours = FindNeighbours(boid, neighbourhoodRadius);

            boid.steering = Vector3.zero;
            boid.steering += SteeringBehaviors.CalculateSeparation(boid, neighbours) * separationForce;

            //Vector3 combinedSteering = SteeringBehaviors.CalculateCohesion

            boid.steering += SteeringBehaviors.CalculateCohesion(boid, neighbours) * cohesionForce * Time.deltaTime;
            boid.steering += SteeringBehaviors.CalculateAlignment(boid, neighbours) * alignmentForce * Time.deltaTime;
            
            float dist = (boid.transform.position - this.transform.position).sqrMagnitude;

            if(dist > flockRadius * flockRadius)
            {
                boid.steering += SteeringBehaviors.CalculateSeek(boid, this.transform.position) * homeForce * Time.deltaTime;
            }
            boid.MoveVehicle();
        }
    }

    void AdjustBoidParameters(Vehicle boid)
    {
        boid.maxSpeed = maxSpeed;
        boid.minSpeed = minSpeed;
        boid.maxSteering = maxSteering;
    }

    List<Vehicle> FindNeighbours(Vehicle vehicle, float radius = 5)
    {
        List<Vehicle> neighbours = new List<Vehicle>();
        Collider[] neighbourColliders = Physics.OverlapSphere(vehicle.transform.position, radius);
        foreach (Collider neighbour in neighbourColliders)
        {
            var boid = neighbour.gameObject.GetComponent<Vehicle>(); //you can use Vehicle instead of var
            if(boid != null)
            {
                neighbours.Add(boid);
            }
        }
        return neighbours;
    }
}
