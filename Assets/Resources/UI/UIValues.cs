﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UIValues : MonoBehaviour
{
    MapIO script;

    string loadFile = "";

    string saveFile = "";
    string mapName = "";
    string prefabSaveFile = "";
    //Todo: Clean this up. It's coarse and rough and irritating and it gets everywhere.
    int mapSize = 1000, mainMenuOptions = 0, toolsOptions = 0, mapToolsOptions = 0, heightMapOptions = 0, conditionalPaintOptions = 0, prefabOptions = 0;
    float heightToSet = 450f, scale = 50f, offset = 0f, mapScale = 1f;
    bool top = false, left = false, right = false, bottom = false, checkHeight = true, setWaterMap = false;
    bool allLayers = false, ground = false, biome = false, alpha = false, topology = false, heightmap = false, prefabs = false, paths = false;
    float heightLow = 0f, heightHigh = 500f, slopeLow = 40f, slopeHigh = 60f;
    float minBlendLow = 25f, maxBlendLow = 40f, minBlendHigh = 60f, maxBlendHigh = 75f, blendStrength = 5f;
    float minBlendLowHeight = 0f, maxBlendHighHeight = 1000f;
    float normaliseLow = 450f, normaliseHigh = 1000f, normaliseBlend = 1f;
    int z1 = 0, z2 = 0, x1 = 0, x2 = 0;
    bool blendSlopes = false, blendHeights = false, aboveTerrain = false;
    int textureFrom, textureToPaint, landLayerFrom, landLayerToPaint;
    int layerConditionalInt, texture = 0;
    bool AlphaVisible = false, AlphaInvisible = false;
    bool TopoActive = false, TopoInactive = false;
    bool deletePrefabs = false;

    bool checkHeightCndtl = false, checkSlopeCndtl = false;
    float slopeLowCndtl = 45f, slopeHighCndtl = 60f;
    float heightLowCndtl = 500f, heightHighCndtl = 600f;

    private TerrainBiome.Enum biomeLayerToPaint;
    private TerrainBiome.Enum biomeLayerConditional;
    private TerrainSplat.Enum groundLayerToPaint;
    private TerrainSplat.Enum groundLayerConditional;

    bool layerSet = false;
    bool[] groundTxtCndtl = new bool[8] { true, true, true, true, true, true, true, true };
    bool[] biomeTxtCndtl = new bool[4] { true, true, true, true };
    bool[] alphaTxtCndtl = new bool[2] { true, true };
    bool[] topoTxtCndtl = new bool[2] { true, true };
    string[] landLayersCndtl = new string[4] { "Ground", "Biome", "Alpha", "Topology" };
    int[] topoLayersCndtl = new int[] { };

    void Start()
    {
         script = GameObject.FindGameObjectWithTag("MapIO").GetComponent<MapIO>();
    }

    public void LoadMap()
    {
        loadFile = UnityEditor.EditorUtility.OpenFilePanel("Import Map File", loadFile, "map");

        var blob = new WorldSerialization();
        if (loadFile == "")
        {
            return;
        }
        EditorUtility.DisplayProgressBar("Loading: " + loadFile, "Loading Land Heightmap Data ", 0.1f);
        blob.Load(loadFile);
        script.loadPath = loadFile;
        EditorUtility.DisplayProgressBar("Loading: " + loadFile, "Loading Land Heightmap Data ", 0.2f);
        script.Load(blob);
    }
    public void SaveMap()
    {
        saveFile = UnityEditor.EditorUtility.SaveFilePanel("Export Map File", saveFile, mapName, "map");
        if (saveFile == "")
        {
            return;
        }
        Debug.Log("Exported map " + saveFile);
        script.savePath = saveFile;
        prefabSaveFile = saveFile;
        EditorUtility.DisplayProgressBar("Saving Map: " + saveFile, "Saving Heightmap ", 0.1f);
        script.Save(saveFile);
    }
    public void NewMap()
    {
        int newMap = EditorUtility.DisplayDialogComplex("Warning", "Creating a new map will remove any unsaved changes to your map.", "Create New Map", "Exit", "Save and Create New Map");
        if (mapSize < 1000 & mapSize > 6000)
        {
            EditorUtility.DisplayDialog("Error", "Map size must be between 1000 - 6000", "Ok");
            return;
        }
        switch (newMap)
        {
            case 0:
                script.loadPath = "New Map";
                script.newEmptyTerrain(mapSize);
                break;
            case 1:
                // User cancelled
                break;
            case 2:
                saveFile = UnityEditor.EditorUtility.SaveFilePanel("Export Map File", saveFile, mapName, "map");
                if (saveFile == "")
                {
                    EditorUtility.DisplayDialog("Error", "Save Path is Empty", "Ok");
                    return;
                }
                Debug.Log("Exported map " + saveFile);
                script.Save(saveFile);
                script.loadPath = "New Map";
                script.newEmptyTerrain(mapSize);
                break;
            default:
                Debug.Log("Create New Map option outofbounds");
                break;
        }
    }
}
