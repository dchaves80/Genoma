using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Control : MonoBehaviour {

    public GameObject OrganismModel;
    public GameObject Target;
    public int Cantidad;
    public float Timecounter=0;
    public float LIFE;
    public List<GameObject> Poblation;
    public bool running=false;
    public List<int> WEIGHTS = new List<int>();
    [SerializeField]
    int weight1 = 0;
    [SerializeField]
    int weight2 = 0;
    [SerializeField]
    List<string[]> ADN1;
    List<string[]> ADN2;


	// Use this for initialization
	void Start () {
        Poblation = new List<GameObject>();
        for (int a = 0; a < Cantidad; a++) 
        {
          GameObject _GO =  Instantiate<GameObject>(OrganismModel);
          ADN _ADN = _GO.GetComponent<ADN>();
          _ADN.Target = Target;
          _ADN.LIFE = LIFE;
          _ADN.transform.position = new Vector3(-35f, 62f, 0f);
          _ADN.InitializeLife(null);
          Poblation.Add(_GO);
          running = true;
        }
	}

    // Update is called once per frame
    void Update()
    {
        Timecounter = Timecounter + Time.deltaTime;
        if (Timecounter > LIFE+0.5f) 
        {
            Timecounter = 0f;
            running = false;
            CalculateResults();        
        }
	
	}

    private void CalculateResults()
    {

        WEIGHTS = new List<int>();
        for (int a = 0; a < Poblation.Count; a++) 
        {
            WEIGHTS.Add(Poblation[a].GetComponent<ADN>().WEIGHT);
        }

    Hello:
        bool sw = false;
        for (int a = 0; a < WEIGHTS.Count; a++) 
        {
            if (a < WEIGHTS.Count - 2) 
            {
                int W1 = WEIGHTS[a];
                int W2 = WEIGHTS[a + 1];
                if (W1 > W2) 
                {
                    WEIGHTS[a] = W2;
                    WEIGHTS[a + 1] = W1;
                    sw = true;
                }
            }

        }
        if (sw == true) 
        {
            goto Hello;
        }


        weight1 = WEIGHTS[WEIGHTS.Count - 1];
        weight2 = WEIGHTS[WEIGHTS.Count - 2];

        

        ADN1 = new List<string[]>();
        ADN2 = new List<string[]>();

        for (int a = 0; a < Poblation.Count; a++)
        {
            if (Poblation[a].GetComponent<ADN>().WEIGHT == weight1) 
            {
                 ADN1 = new List<string[]>( Poblation[a].GetComponent<ADN>().MyADN);
            };

            if (Poblation[a].GetComponent<ADN>().WEIGHT == weight2)
            {
                ADN2 = new List<string[]>( Poblation[a].GetComponent<ADN>().MyADN);
            };
        }

     

        bool change = false;

        for (int a = 0; a < ADN1.Count; a++) 
        {
            for (int b = 0; b < ADN1[a].Length; b++) 
            {
                int RR = Random.Range(0, 10);

                if (change == true && RR==3)
                {
                    string DNA1 = ADN1[a][b];
                    string DNA2 = ADN2[a][b];

                    ADN1[a][b] = DNA2;
                    ADN2[a][b] = DNA1;
                    change = false;
                }
                else 
                {
                    ADN1[a][b] = ADN1[a][b];
                    ADN2[a][b] = ADN2[a][b];
                    change = true;
                }
            }
        }

        GameObject[] GOL = GameObject.FindGameObjectsWithTag("Organismo");
        for (int a = 0; a < GOL.Length; a++) 
        {
            DestroyImmediate(GOL[a]);
        }

        Poblation.Clear();
        Poblation = new List<GameObject>();
        bool divide = false;
        for (int a = 0; a < Cantidad; a++)
        {
            GameObject _GO = Instantiate<GameObject>(OrganismModel);
            ADN _ADN = _GO.GetComponent<ADN>();
            _ADN.Target = Target;
            _ADN.LIFE = LIFE;
            _ADN.transform.position = new Vector3(-35f, 62f, 0f);
            if (divide == true)
            {
                _ADN.InitializeLife(ADN1, a == 0 || a == 1);
                divide = false;
            }
            else 
            {
                _ADN.InitializeLife(ADN2, a == 0 || a == 1);
                divide = true;
            }
            Poblation.Add(_GO);
            
        }
        
        running = true;

    }






}
