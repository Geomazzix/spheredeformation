using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MeshSaver : MonoBehaviour
{
    private string _FilePath;
    private string _FileName;

    //Set the destination of the file.
    public void SetFilePath(string filePath = null)
    {
        if(filePath != null)
        {
            _FilePath = filePath;
        }
        else
        {
            _FilePath = Application.dataPath;
        }
    }

    public void SetFileName(string fileName = null)
    {
        if(fileName == null)
        {
            _FileName = fileName + ".obj";
        }
        else
        {
            _FileName = "AtLeastGiveItANameJeez.obj";
        }
    }

    //Writes data in the newly generated meshfile.
    public void WriteMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if (meshFilter != null)
        {
            if (meshFilter.sharedMesh != null)
            {
                //Create a streamwriter for saving the mesh data.
                using (StreamWriter meshWriter = new StreamWriter(_FilePath + "/sphere.obj"))
                {
                    print(_FilePath);

                    //Command lines.
                    meshWriter.WriteLine("# Tom Lemmers copyright");

                    //Vertice array length.
                    meshWriter.WriteLine("# " + meshFilter.sharedMesh.vertices.Length + " vertices");

                    //Write the vertices in the file.
                    for (int i = 0; i < meshFilter.sharedMesh.vertices.Length; i++)
                    {
                        meshWriter.WriteLine("v " + meshFilter.sharedMesh.vertices[i].x + " " + meshFilter.sharedMesh.vertices[i].y + " " + meshFilter.sharedMesh.vertices[i].z);
                    }

                    //Triangle array length.
                    meshWriter.WriteLine("# " + meshFilter.sharedMesh.triangles.Length + " triangles");

                    //Write the triangles in the file.
                    for (int i = 0; i < meshFilter.sharedMesh.triangles.Length; i += 3)
                    {
                        meshWriter.WriteLine("f " + (meshFilter.sharedMesh.triangles[i] + 1) + " " + (meshFilter.sharedMesh.triangles[i + 1] + 1) + " " + (meshFilter.sharedMesh.triangles[i + 2] + 1));
                    }
                }
            }
            else
            {
                Debug.LogError("The shared mesh of the meshfilter has not been generated yet.");
            }
        }
        else
        {
            Debug.LogError("The object needs at least a meshfilter and a meshrenderer.");
        }
    }
}
