using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerEnemys : MonoBehaviour
{
    public static PlayerPowerEnemys Instance;

    [BoxGroup("Destroys")][SerializeField, Range(0f, 100f)] private float _flyPercent;
    [BoxGroup("Destroys")][SerializeField] private float _flyCenterPower;
    [BoxGroup("Destroys")][SerializeField] private float _flyMovePower;
    [BoxGroup("Destroys")][SerializeField] private float _flyUpPower;
    [BoxGroup("Destroys")][SerializeField, MinMaxSlider(0f, 1000f)] private Vector2 _flyRotationPower;
    [BoxGroup("Destroys")][SerializeField] private float _flyTime;

    [SerializeField] private PlayerLayer[] Layers;
    [BoxGroup("Debug")][ReadOnly][SerializeField] private int LayerId = 0;

    [System.Serializable]
    public class PlayerLayer
    {
        public float Radius = 1.2f;
        public int EnemyCount = 20;
        public GameObject LayerObject;
        public int EnemyUsed = 0;
        public Vector3[] Positions;
        public List<Transform> Objects;
    }

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < Layers.Length; i++)
        {
            Layers[i].LayerObject.SetActive(i == LayerId);
            Layers[i].Positions = CreatePosition(Layers[i]);
            Layers[i].Objects = new List<Transform>();
        }
    }

    public void AddUnit(Transform obj)
    {
        if (Layers[Layers.Length - 1].EnemyUsed < Layers[Layers.Length - 1].EnemyCount)
        {
            obj.SetParent(GetActiveLayer());
            Layers[LayerId].Objects.Add(obj);
            obj.localPosition = GetNextPos();
            obj.LookAt(transform.position);
            obj.Rotate(new Vector3(0.2f, 1f, 0.2f), Random.Range(-180f, 180f), Space.Self);
        }
        else
        {
            Destroy(obj.gameObject);
        }
    }

    public void RemoveUnit(float percent)
    {
        List<Transform> removeList = new List<Transform>();
        int allUnits = 0;
        for (int i = 0; i < Layers.Length; i++)
        {
            allUnits += Layers[i].EnemyUsed;
        }
        int removeUnits = Mathf.RoundToInt((float)allUnits * percent);
        for (int i = Layers.Length - 1; i >= 0; i--)
        {
            for (int j = Layers[i].EnemyUsed - 1; j >= 0 && removeUnits > 0; j--)
            {
                //Debug.Log("Layer: " + i.ToString() + "; EnemyUsed: " + Layers[i].EnemyUsed.ToString() + "->" + (Layers[i].EnemyUsed - 1).ToString() + "; removeUnitsLeft: " + removeUnits.ToString());
                if (Layers[i].Objects[^1] != null)
                {
                    removeList.Add(Layers[i].Objects[^1].transform);
                    removeUnits--;
                }
                Layers[i].Objects.RemoveAt(Layers[i].EnemyUsed - 1);
                Layers[i].EnemyUsed--;
            }
        }
        LayerId = 0;
        for (int i = 0; i < Layers.Length; i++)
        {
            if (Layers[i].EnemyUsed > 0)
            {
                LayerId = i;
            }
        }
        for (int i = 0; i < Layers.Length; i++)
        {
            Layers[i].LayerObject.SetActive(i <= LayerId);
        }

        int flyCount = Mathf.RoundToInt((float)removeList.Count * (_flyPercent * 0.01f));
        for (int i = 0; i < removeList.Count; i++)
        {
            if (i < flyCount)
            {
                Transform obj = removeList[i];
                obj.SetParent(null);
                Rigidbody objRigid;
                if (obj.GetComponent<Rigidbody>())
                {
                    objRigid = obj.GetComponent<Rigidbody>();
                }
                else
                {
                    objRigid = obj.gameObject.AddComponent<Rigidbody>();
                }
                Vector3 dirCenter = (obj.position - transform.position).normalized * _flyCenterPower;
                Vector3 dirMove = PlayerController.Controller.GetVelocity().normalized;
                dirMove.y = 0f;
                dirMove *= _flyMovePower;

                Vector3 rotPower = new Vector3(Random.Range(_flyRotationPower.x, _flyRotationPower.y), Random.Range(_flyRotationPower.x, _flyRotationPower.y), Random.Range(_flyRotationPower.x, _flyRotationPower.y));

                objRigid.velocity = dirCenter + dirMove + new Vector3(0f, _flyUpPower, 0f);
                objRigid.angularVelocity = rotPower * Time.fixedDeltaTime;

                Destroy(obj.gameObject, _flyTime);
            }
            else
            {
                Destroy(removeList[i].gameObject, 0.05f);
            }
        }
    }

    public Vector3 GetNextPos()
    {
        Vector3 targetPos = Vector3.zero;
        if (LayerId < Layers.Length)
        {
            targetPos = Layers[LayerId].Positions[Layers[LayerId].EnemyUsed];
            Layers[LayerId].EnemyUsed++;
            if (Layers[LayerId].EnemyUsed == Layers[LayerId].EnemyCount)
            {
                //if (LayerId < Layers.Length - 1)
                //    Layers[LayerId].LayerObject.SetActive(false);
                LayerId++;
                if (LayerId < Layers.Length)
                    Layers[LayerId].LayerObject.SetActive(true);
            }
        }
        return targetPos;
    }

    public Transform GetActiveLayer()
    {
        if (LayerId < Layers.Length)
        {
            return Layers[LayerId].LayerObject.transform;
        }
        return transform;
    }

    public SphereCollider GetActiveCollider()
    {
        if (LayerId < Layers.Length)
        {
            return Layers[LayerId].LayerObject.transform.GetChild(0).GetComponent<SphereCollider>();
        }
        return transform.GetComponentInChildren<SphereCollider>();
    }

    private Vector3[] CreatePosition(PlayerLayer layer)
    {
        List<Vector3> positions = new List<Vector3>();
        int numSpirals = Mathf.CeilToInt(0.5f * (-1 + Mathf.Sqrt(1 + 8 * layer.EnemyCount)));

        float phi = Mathf.PI * (3f - Mathf.Sqrt(5f)); // Золотое сечение

        for (int i = 0; i < layer.EnemyCount; i++)
        {
            float y = 1f - (i / (float)(layer.EnemyCount - 1)) * 2f; // Интерполируем значения y от -1 до 1

            float radiusAtY = Mathf.Sqrt(1 - y * y); // Радиус окружности на уровне y

            float theta = phi * i; // Угол вокруг оси y

            float x = Mathf.Cos(theta) * radiusAtY;
            float z = Mathf.Sin(theta) * radiusAtY;

            Vector3 position = new Vector3(x, y, z) * layer.Radius;
            positions.Add(position);
        }
        return positions.ToArray();
    }

    public float GetRadius()
    {
        return Layers[LayerId].Radius;
    }

#if UNITY_EDITOR

    /*[BoxGroup("Debug")][SerializeField] private bool DebugAllLayer = false;
    [BoxGroup("Debug")][DisableIf("DebugAllLayer")][SerializeField] private int DebugLayer = 0;

    private void OnDrawGizmosSelected()
    {
        DebugLayer = Mathf.Clamp(DebugLayer, 0, Layers.Length - 1);
        for (int i = 0; i < Layers.Length; i++)
        {
            if (DebugAllLayer)
            {
                GizmosDrawLayer(i, true);
            }
            else
            {
                GizmosDrawLayer(i, i == DebugLayer);
            }
        }
    }

    private void GizmosDrawLayer(int i, bool objActive)
    {
        if (!Application.isPlaying)
            Layers[i].LayerObject.SetActive(objActive);

        if (objActive)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, Layers[i].Radius);

            Gizmos.color = Color.yellow;
            Layers[i].Positions = CreatePosition(Layers[i]);
            for (int j = 0; j < Layers[i].Positions.Length; j++)
            {
                Gizmos.DrawSphere(Layers[i].Positions[j] + transform.position, 0.05f);
            }
        }
    }*/

#endif

}
