using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class OctahedronAlgorithm : MonoBehaviour
{
    private float _Radius;
    private int _SubDivisions;

    //Make sure to generate the octahedron.
    private void Awake()
    {
        _Radius = 1f;
        _SubDivisions = 0;
        GenerateSphere();
    }

    //Creates a mesh.
    public void GenerateSphere()
    {
        //Get componments of the mesh componments.
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();

        //Set the vertices for the smallest octahedron, existing out 2 piramids on top of each other.
        List<Vector3> vertices = new List<Vector3>
        {
            new Vector3(-1, 0, -1),  //0
            new Vector3(-1, 0, 1),   //1
            new Vector3(1, 0, 1),   //2
            new Vector3(1, 0, -1),  //3
            new Vector3(0, 1, 0),   //4
            new Vector3(0, -1, 0)   //5
        };


        //Normal list.
        Vector3[] normals;


        //Set the indices of the triangles for the smallest octahedron.
        List<int> indices = new List<int>
        {
            //Upside
            4, 0, 1,
            4, 1, 2,
            4, 2, 3,
            4, 3, 0,

            //Downside
            5, 0, 3,
            5, 2, 1,
            5, 3, 2,
            5, 1, 0
        };

        //Sets a count for how many times the inner triangles should form, this can only be a maximum of 6 because the mesh gets too complicated for the 
        //unity game engine to handle.
        for (int i = 0; i < _SubDivisions; i++)
        {
            //Overwrite the old list with the new list of triangles.
            indices = CalculateInnerTriangle(indices, vertices);
        }

        //Calculate the normals of the laptop.
        //normals = CalculateNormals(indices, vertices);

        //Give the mesh a spherical shape.
        ShapeSphere(vertices);


        //Assign the arrays in the new mesh.
        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();
        //mesh.normals = normals;
        mesh.RecalculateNormals();


        //Asign the new mesh to the meshFilter so it will get projected.
        meshFilter.sharedMesh = mesh;
    }


    //Calculates the normals of the mesh.
    public Vector3[] CalculateNormals(List<int> indices, List<Vector3> vertices)
    {
        Vector3[] calculatedNormals = new Vector3[vertices.Count];

        for (int i = 0; i < vertices.Count; i++)
        {
            calculatedNormals[i] = (vertices[i] - transform.position).normalized;
        }

        /*
         * Calculates the normal faces.
         * 
         * 
        Vector3[] calculatedNormals = new Vector3[indices.Count / 3];
        
        for (int i = 0; i < indices.Count; i+=3)
        {
            Vector3 triangleCenter = (vertices[indices[i]] + vertices[indices[i + 1]] + vertices[indices[i + 2]]) / 3;
            calculatedNormals[i / 3] = (transform.position - triangleCenter).normalized;
        }
        */
        return calculatedNormals;
    }


    //Calculate the inner triangle.
    private List<int> CalculateInnerTriangle(List<int> indices, List<Vector3> vertices)
    {
        //Create a new list to store the new triangle indices in.
        List<int> newIndices = new List<int>();

        for (int i = 0; i < indices.Count; i += 3)
        {
            //Get the last 3 indices of the indices array.
            Vector3 v0 = vertices[indices[i]];
            Vector3 v1 = vertices[indices[i + 1]];
            Vector3 v2 = vertices[indices[i + 2]];

            //Make sure to add the new vertices to the list.
            vertices.Add((v0 + v1) / 2);
            vertices.Add((v1 + v2) / 2);
            vertices.Add((v2 + v0) / 2);

            //Upper triangle.
            newIndices.Add(indices[i]);
            newIndices.Add(vertices.Count - 3);
            newIndices.Add(vertices.Count - 1);

            //Right triangle.
            newIndices.Add(vertices.Count - 3);
            newIndices.Add(indices[i + 1]);
            newIndices.Add(vertices.Count - 2);

            //Left triangle.
            newIndices.Add(vertices.Count - 1);
            newIndices.Add(vertices.Count - 2);
            newIndices.Add(indices[i + 2]);

            //Center triangle.
            newIndices.Add(vertices.Count - 3);
            newIndices.Add(vertices.Count - 2);
            newIndices.Add(vertices.Count - 1);
        }

        //Return the new list to overwrite the old one.
        return newIndices;
    }


    //Sets the sphereshape to the mesh.
    private void ShapeSphere(List<Vector3> vertices)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] = ((vertices[i] - transform.position).normalized * _Radius);
        }
    }


    //change the radius of the sphere.
    public void ChangeRadius(float radius)
    {
        _Radius = radius;
    }


    //Changes the count of subdivisions.
    public void ChangeSubdivisions(int subdivs)
    {
        _SubDivisions = subdivs;
    }
}
