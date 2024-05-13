using UnityEngine;
using UnityEditor;

public class FixNormals
{
    /*
     * Set the normals for each blend shape frame to the
     * same normals as the base mesh and export a new mesh
     * asset with a sensible name. Select a mesh, right
     * click and choose 'Generate Mesh With Fixed Normals'.
     */
    [MenuItem("Assets/Generate Mesh With Fixed Normals")]
    private static void FixBlendShapeNormals()
    {

        if (Selection.activeObject.GetType() != typeof(Mesh))
        {
            Debug.LogError("This isn't a mesh.");
            return;
        }

        Debug.Log("let's fix these blendshape normals!");

        Mesh selected = Selection.activeObject as Mesh;

        Vector3[] deltaVertices = new Vector3[selected.vertexCount];
        Vector3[] deltaNormals = new Vector3[selected.vertexCount];
        Vector3[] deltaTangents = new Vector3[selected.vertexCount];

        int bsc = selected.blendShapeCount;

        Mesh newMesh = new Mesh();
        newMesh.vertices = selected.vertices;
        newMesh.uv = selected.uv;
        newMesh.normals = selected.normals;
        newMesh.colors = selected.colors;
        newMesh.tangents = selected.tangents;
        newMesh.subMeshCount = selected.subMeshCount;
        //newMesh.triangles = selected.triangles;

        int subMeshes = selected.subMeshCount;
        for (int i = 0; i < subMeshes; i++)
        {
            int[] tris = selected.GetTriangles(i);
            newMesh.SetIndices(tris, MeshTopology.Triangles, i);
        }

        newMesh.name = selected.name + "_fixed";
        newMesh.boneWeights = selected.boneWeights;
        newMesh.bindposes = selected.bindposes;

        for (int i = 0; i < bsc; i++)
        {
            string name = selected.GetBlendShapeName(i);
            int weightCount = selected.GetBlendShapeFrameCount(i);
            for (int j = 0; j < weightCount; j++)
            {
                float weight = selected.GetBlendShapeFrameWeight(i, j);
                selected.GetBlendShapeFrameVertices(i, j, deltaVertices, deltaNormals, deltaTangents);
                newMesh.AddBlendShapeFrame(name, weight, deltaVertices, selected.normals, deltaTangents);
            }
        }

        string savePath = AssetDatabase.GetAssetPath(selected);
        savePath = savePath.Substring(0, savePath.LastIndexOf('/') + 1);

        string newAssetName = savePath + selected.name + "_fixed.asset";

        AssetDatabase.CreateAsset(newMesh, newAssetName);

        AssetDatabase.SaveAssets();

        Debug.Log("Done!");
    }
}

