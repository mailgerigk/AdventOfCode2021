using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject Prefab;
    public GameObject Explosion;

    public float waitForDataUpdate;
    public float waitForExplosion;
    public float spaceingX = 1;
    public float spaceingY = 1;

    GameObject[][] grid;
    int[][] data;
    bool isStepping;

    public int tick;
    public int flashCount;
    public int firstAllFlash;

    public Color lerpFrom;
    public Color lerpTo;

    // Start is called before the first frame update
    void Start()
    {
        var lines = File.ReadAllLines("Assets/Resources/input.txt");
        grid = new GameObject[lines.Length][];
        data = new int[lines.Length][];
        for (int y = 0; y < lines.Length; y++)
        {
            grid[y] = new GameObject[lines[y].Length];
            data[y] = new int[lines[y].Length];
            for (int x = 0; x < lines[y].Length; x++)
            {
                grid[y][x] = Instantiate(Prefab, Vector3.back * y * spaceingY + Vector3.right * x * spaceingX, Quaternion.identity);
                data[y][x] = lines[y][x] - '0';
            }
        }
    }

    IEnumerator step()
    {
        isStepping = true;

        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                data[y][x]++;
            }
        }

        yield return new WaitForSeconds(waitForDataUpdate);

        bool changed = true;
        int flashCountBefore = flashCount;
        while (changed)
        {
            changed = false;
            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    if (data[y][x] > 9)
                    {
                        changed = true;
                        flashCount++;

                        data[y][x] = 0;
                        Instantiate(Explosion, grid[y][x].transform).GetComponent<Explosion>().time = waitForExplosion;

                        for (int dy = -1; dy < 2; dy++)
                        {
                            for (int dx = -1; dx < 2; dx++)
                            {
                                if (0 <= y + dy && y + dy < data.Length && 0 <= x + dx && x + dx < data[y + dy].Length && data[y + dy][x + dx] != 0)
                                {
                                    data[y + dy][x + dx]++;
                                }
                            }
                        }
                    }
                }
            }
            if (changed)
            {
                yield return new WaitForSeconds(waitForExplosion);
            }
        }

        if (firstAllFlash == 0 && flashCount - flashCountBefore == data.Length * data[0].Length)
        {
            firstAllFlash = 1 + tick;
        }

        isStepping = false;

        Debug.Log($"{++tick}: {flashCount}");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStepping)
        {
            StartCoroutine(step());
        }

        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                grid[y][x].GetComponent<MeshRenderer>().material.color = Color.Lerp(lerpFrom, lerpTo, (float)data[y][x] / 9.0f);
                grid[y][x].transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material.color = Color.Lerp(lerpFrom, lerpTo, (float)data[y][x] / 9.0f);
            }
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(15, 15, 200, 200), $"Step: {tick} Flashes: {flashCount}");
    }
}
