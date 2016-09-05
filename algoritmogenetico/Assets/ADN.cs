using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ADN : MonoBehaviour {

    public List<string[]> MyADN;
    public float LIFE=10f;
    public int WEIGHT=0;
    public bool GetLife=true;
    public float wait = 0f;
    public float counterwait = 0f;
    public int positionfila=0;
    public int positioncolumna=0;
    public GameObject Target;
    public Rigidbody2D MyRB;
    public bool AutoStart=false;
    [TextArea(3, 10)]
    public string MyADNString="";
    public string currentGEN = "";
    

    public void InitializeLife(List<string[]> p_adninitialize, bool Mutation=false)
    {
        MyADN = new List<string[]>();
        this.gameObject.tag = "Organismo";
        if (p_adninitialize == null)
        {
            MyADN.Add(getNewRandomGEN());
            MyADN.Add(getNewRandomGEN());
            MyADN.Add(getNewRandomGEN());
            MyADN.Add(getNewRandomGEN());
            MyADN.Add(getNewRandomGEN());
            MyADN.Add(getNewRandomGEN());
            //Mutate();
            
            
        }
        else 
        {
            for (int a = 0; a < p_adninitialize.Count; a++) 
            {
                string[] Chain=new string[8]; 
                
                p_adninitialize[a].CopyTo(Chain,0);

                
                MyADN.Add(Chain);
                //MyADN.Add(getNewRandomGEN());
                if (Mutation == true)
                {
                    Mutate();
                }
                
            }
        }

        for (int a = 0; a < MyADN.Count; a++) 
        {
            for (int b = 0; b < MyADN[a].Length; b++) 
            {
                MyADNString = MyADNString + "[" + MyADN[a][b] + "]";
            }
        }
        this.enabled = true;

    }

    // Use this for initialization
    void Start()
    {
        MyRB = gameObject.GetComponent<Rigidbody2D>();
        if (AutoStart) 
        {
            InitializeLife(null);
        }
	}


    void ReadGen(string p_Gen) 
    {
        char[] split = {'/'};
        string[] decodegen = p_Gen.Split(split);

        string Action = decodegen[0];
        float Time = float.Parse( decodegen[1]);
        float Force =float.Parse( decodegen[2]);
        

        switch (Action)
        {
            case "F":
                MyRB.AddRelativeForce(new Vector2(Force,0));
                break;
            case "B":
                MyRB.AddRelativeForce(new Vector2(-Force,0));
                break;
            case "R":
                MyRB.AddTorque(Force);
                break;
            case "L":
                MyRB.AddTorque(-Force);
                break;
        }
        wait = Time;
    }

	// Update is called once per frame
	void Update () {

        

        if (LIFE > 0)
        {
            LIFE = LIFE - Time.deltaTime;
            counterwait=counterwait+Time.deltaTime;
            if (counterwait > wait)
            {
                counterwait = 0f;
                if (positioncolumna > 7)
                {
                    positioncolumna = 0;
                    positionfila++;
                }
                if (positionfila > MyADN.Count - 1) positionfila = 0;
                currentGEN = MyADN[positionfila][positioncolumna];
                ReadGen(MyADN[positionfila][positioncolumna]);
                positioncolumna++;
            }



        }
        else 
        {
            MyRB.isKinematic = true;
            GetLife = false;
            WEIGHT = Mathf.RoundToInt( (1f / Vector3.Distance(this.transform.position, Target.transform.position))*10000);
            this.enabled = false;
        }
	
	}

    void Mutate()
    {
        Random.seed = System.DateTime.Now.Millisecond;
        System.Random R = new System.Random(Random.Range(1, int.MaxValue));
        for (int a=0;a<MyADN.Count;a++){
            for (int b = 0; b < MyADN[a].Length; b++)
            {

               
                int Fila = a;
                int Columna = b;
                int MustMutate = R.Next(0, 10);

                if (MustMutate == 3 || MustMutate==7)
                {

                    int MyRandomNumber = R.Next(0, 5);
                    string action = "";
                    float factortorque = 1f;
                    switch (MyRandomNumber)
                    {
                        case 0:
                            action = "N";
                            break;
                        case 1:
                            action = "F";
                            break;
                        case 2:
                            action = "B";
                            break;
                        case 3:
                            action = "R";
                            factortorque = 10f;
                            break;
                        case 4:
                            action = "L";
                            factortorque = 10f;
                            break;

                    }
                    float Time = R.Next(1, 1000) / 10000f;
                    float Force = (R.Next(1, 10) / 5) * factortorque;

                    MyADN[Fila][Columna] = action + "/" + Time.ToString() + "/" + Force.ToString();
                }
            }

        }
    }

    private string[] getNewRandomGEN()
    {
        System.Random R = new System.Random(Random.Range(1,int.MaxValue));

        string[] newADNCHain = { "N/0/0", "N/0/0", "N/0/0", "N/0/0", "N/0/0", "N/0/0", "N/0/0", "N/0/0", };
        
        for (int a = 0; a < newADNCHain.Length; a++)
        {
            int MyRandomNumber = R.Next(0, 5);
            string action = "";
            float factorTorque = 0f;

            switch (MyRandomNumber)
            {
                case 0:
                    action = "N";
                    factorTorque = 0f;
                    break;
                case 1:
                    action = "F";
                    factorTorque = 1f;
                    break;
                case 2:
                    action = "B";
                    factorTorque = 1f;
                    break;
                case 3:
                    action = "R";
                    factorTorque = 10f;
                    break;
                case 4:
                    action = "L";
                    factorTorque = 10f;
                    break;

            }



            float Time = R.Next(1,1000)/10000f;
            float Force = (R.Next(1, 10) / 5f)*factorTorque;

            newADNCHain[a] = action + "/" + Time.ToString() + "/" + Force.ToString();

        }

        return newADNCHain;
    }
}
