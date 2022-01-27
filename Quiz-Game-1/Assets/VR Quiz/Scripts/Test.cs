using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("The scoreBoard script used to update the score of the player")]
    public ScoreBoard scbScript;
    public Text scoreTxt_inScoreBoard;

    [Header("Start Button")]
    public GameObject startButton;

    [Header("Question time")]
    public int questionTime = 15;

    [Header("Name of the text files used in the program")]
    public string questionTXTname;
    public string answerTXTname;
    public string spritesTXTname;
    public string gameObjectsTXTname;

    [Header("Arrays of data where QEST, ANW, SPRITES and OBJECTS are stored")]
    public List<string> questions;
    public List<string> answers;
    public string[] answers1;
    public string[] answers2;
    public string[] answers3;
    public string[] answers4;

    public List<string> sptritesRef;
    public Sprite[] sprites;

    public List<string> gameObj3dRef;
    public GameObject[] gameObj3D;

    [Header("Reference to the TEXT and IMAGE where things will be displayed")]
    public Text question_txt;
    public Text answ1_txt;
    public Text answ2_txt;
    public Text answ3_txt;
    public Text answ4_txt;
    public Image logo_img;

    [Header("Image that appears by default in the test if none is selected")]
    public Sprite logo;

    [Header("Arrays of data where QEST, ANW, SPRITES and OBJECTS are stored")]
    public Transform gameObjectPosition;

    [Header("Correct indices for the answers")]
    public int[] correct;

    [Header("The four selectors that appear on top of the selected answer")]
    public GameObject[] selectors;


    [Header("Correct indices for the answers")]
    public Text time_txt;
    public Image time_img;

    [Header("Storing indices of questions")]
    public int actualQuestion=0;
    public int nb_questions = 0;
    int selectedAnsw = -1;

    [Header("Answers that are OK and INCORRECT")]
    public int answOK;
    public int answNO;

    [Header("TEXT to display results")]
    public Text answOK_txt;
    public Text answNO_txt;

    public GameObject goOk, goNO;

    [Header("Result variables after closing the test ")]
    public Text scoreTxt;
    public Text finalGoodTxt;
    public Text finalBadTxt;
    public GameObject finalScoreGO;
    public int score;

    //private variables
    TextAsset questionsAsset;
    TextAsset answersAsset;
    TextAsset spritesAsset;
    TextAsset gameObj3dAsset;




    float elapsed = 0;
    bool ready=true;

    //when enabeling the gameobject, perform these operations
    void OnEnable()
    {
        finalScoreGO.SetActive(false);

        //load the text assets
        questionsAsset =Resources.Load(questionTXTname) as TextAsset;
        answersAsset = Resources.Load(answerTXTname) as TextAsset;
        spritesAsset = Resources.Load(spritesTXTname) as TextAsset;
        gameObj3dAsset = Resources.Load(gameObjectsTXTname) as TextAsset;

        //save in a array of strings
        questions = questionsAsset.text.Split('\n').ToList();
        answers = answersAsset.text.Split('\n').ToList();
        sptritesRef= spritesAsset.text.Split('\n').ToList();
        gameObj3dRef = gameObj3dAsset.text.Split('\n').ToList();

        //this is the total number of questions for the test
        nb_questions = questions.Count;

        //strings used to store data
        answers1 = new string[answers.Count/5];
        answers2 = new string[answers.Count / 5];
        answers3 = new string[answers.Count / 5];
        answers4 = new string[answers.Count / 5];
        correct = new int[answers.Count / 5];
        sprites = new Sprite[nb_questions];
        gameObj3D = new GameObject[nb_questions];

        //for each pack of 5 lines, get the answers and correct answers
        /*
         * Aswers follow the structure of:
         *         TITLE
         *         answer1
         *         answer2
         *         answer3
         *         $coorrect answer4
         * 
         */

        //set the sprites
        for (int jj = 0; jj < nb_questions; jj++)
        {
            sptritesRef[jj] = sptritesRef[jj].Substring(0, sptritesRef[jj].Length-1);

            sprites[jj] = Resources.Load<Sprite>("Sprites/" + sptritesRef[jj]);

            if (sprites[jj]==null)
            {
                sprites[jj] =logo;
            }
           
            
        }


        //set the 
        for (int jj = 0; jj < nb_questions; jj++)
        {
            gameObj3dRef[jj] = gameObj3dRef[jj].Substring(0, gameObj3dRef[jj].Length - 1);
            gameObj3D[jj] = Resources.Load<GameObject>("GameObjects/" + gameObj3dRef[jj]);
            

        }




        for (int ii=0;ii< answers.Count / 5;ii++)
        {
             answers1[ii] =answers[ii*5+1];
             answers2[ii] = answers[ii * 5 + 2];
             answers3[ii] = answers[ii * 5 + 3];
             answers4[ii] = answers[ii * 5 + 4];

            if(answers1[ii][0]=='$')
            {
                answers1[ii] = answers1[ii].Substring(1);
                correct[ii] = 0;
            }
            if (answers2[ii][0] == '$')
            {
                answers2[ii] = answers2[ii].Substring(1);
                correct[ii] = 1;
            }
            if (answers3[ii][0] == '$')
            {
                answers3[ii] = answers3[ii].Substring(1);
                correct[ii] = 2;
            }
            if (answers4[ii][0] == '$')
            {
                answers4[ii] = answers4[ii].Substring(1);
                correct[ii] = 3;
            }


        }


      

        //we start with no answer selection
        selectedAnsw = -1;

        //we reset the counters
        answOK = 0;
        answNO = 0;
            

        // set the actual question
        actualQuestion = 0;
        changeQuestion(0);

        
    }


    //quit the test function
    public void quitTest()
    {
       
        ready = true;

        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //display the results on the right
        answOK_txt.text =""+answOK;
        answNO_txt.text = "" + answNO;

        //if it is ready,
        if (ready==true)
        {
            //time counter
            elapsed += Time.fixedDeltaTime;
            time_txt.text = "" + Mathf.Round(questionTime - elapsed);
            time_img.fillAmount = (questionTime - elapsed) / questionTime;

            //go to next question if time exceeds the given time
            if (elapsed > questionTime)
            {
                actualQuestion += 1;
                elapsed = 0;
                ready = false;


                StartCoroutine(prepareNextQuestion());
            }

        }
    }


    //this function take sthe user to a given question "a"
    public void changeQuestion(int a)
    {

        question_txt.text =questions[a];
        answ1_txt.text = answers1[a];
        answ2_txt.text = answers2[a];
        answ3_txt.text = answers3[a];
        answ4_txt.text = answers4[a];

        //isntatiate the gameobject if exists
        if(gameObj3D[a]!=null)
        {
            GameObject goInst=Instantiate(gameObj3D[a]);
            goInst.transform.position = gameObjectPosition.position;

            Destroy(goInst, questionTime);
        }

        logo_img.sprite = sprites[a];
    }

    //this function avtivates the selection on top of an answer
    public void select(int a)
    {
        selectedAnsw =a;

        for (int ii=0;ii<selectors.Length;ii++)
        {
            selectors[ii].SetActive(false);
        }

        selectors[a].SetActive(true);
    }



    //this corrutine prepares the next question
    public IEnumerator prepareNextQuestion()
    {
        for (int ii = 0; ii < selectors.Length; ii++)
        {
            selectors[ii].SetActive(false);
        }
        

        float elapsed = 0;

        //increment the number of correct and incorrect questions
        if(selectedAnsw==correct[actualQuestion-1])
        {
            answOK += 1;
            goOk.SetActive(true);
        }
        else
        {
            answNO += 1;
            goNO.SetActive(true);
        }


       //wait 2 seconds
        while (elapsed<2)
        {
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }


        goOk.SetActive(false);
        goNO.SetActive(false);

        //test is finished
        if (actualQuestion > nb_questions-1)
        {
            ready = true;

            gameObject.SetActive(false);

            startButton.SetActive(true);

            //set the final score and answers
            finalGoodTxt.text = "" + answOK;
            finalBadTxt.text = "" + answNO;

            score = (answOK * 1000 - answNO * 0);

            //do not allow negative scores
            if(score<0)
            {
                score = 0;
            }

            scoreTxt.text = "SCORE: "+score;

            scoreTxt_inScoreBoard.text = "" + score;

            finalScoreGO.SetActive(true);

            //apply order in array
            scbScript.udpateScores();


        }
        //go to next queswtion
        else
        {
            changeQuestion(actualQuestion);

            selectedAnsw = -1;
        }
        ready = true;
    }




}

