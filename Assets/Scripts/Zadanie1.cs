using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class Zadanie1 : MonoBehaviour
{
    public NativeArray<int> Array;
    public string Result;

    public struct MyJob : IJob
    {
        public NativeArray<int> Numbers;
        public void Execute() 
        { 
            for (int i = 0; i < Numbers.Length; i++)
            {
                if (Numbers[i] > 10) Numbers[i] = 0;
            }
        }
    }

    void Start()
    {
        Array = new NativeArray<int>(20,Allocator.Persistent);
        for (int i = 0; i < Array.Length; i++) {Array[i] = i+1;}

        Result = "";

        for (int i = 0; i < Array.Length; i++) { Result += Array[i] + " "; }
        Debug.Log("Массив до: \n" + Result);

        MyJob myJob = new MyJob()
        {
            Numbers = Array
        };

        JobHandle jobHandle = myJob.Schedule();
        jobHandle.Complete();

        Result = "";
        for (int i = 0; i < Array.Length; i++) { Result += Array[i] + " ";}
        Debug.Log("Массив после: \n" + Result);
        Array.Dispose();
    }
}

