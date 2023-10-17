using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NNDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text generationText;
    [SerializeField] TMP_Text highestGenText;

    float highestFitness = float.MinValue, highestFitnessThisGen = float.MinValue;
    int currentGeneration = 0, highestScoreGen = 0;

    void Awake()
    {
        if (FindAnyObjectByType<NNTrainer>() == null)
            return;

        NNTrainer trainer = FindObjectOfType<NNTrainer>();
        trainer.GenerationStarted += HandleGenerationStarted;
        trainer.HighestFitnessChanged += HandleHighestFitnessChanged;
        trainer.HighestFitnessThisGenChanged += HandleHighestFitnessThisGenChanged;

        generationText.text = $"Generation: {currentGeneration}\n" +
            $"Best Last Gen: {highestFitnessThisGen}";
        highestGenText.text = $"Highest Fitness: {highestFitness}\n" +
            $"From Gen: {highestScoreGen}";
    }

    private void HandleHighestFitnessThisGenChanged(object sender, float e)
    {
        highestFitnessThisGen = e;
        generationText.text = $"Generation: {currentGeneration}\n" +
            $"Best Last Gen: {highestFitnessThisGen}";
    }

    private void HandleHighestFitnessChanged(object sender, float e)
    {
        highestFitness = e;
        highestScoreGen = currentGeneration;
        highestGenText.text = $"Highest Fitness: {highestFitness}\n" +
            $"From Gen: {highestScoreGen}";
    }

    private void HandleGenerationStarted(object sender, int e)
    {
        currentGeneration = e;
        generationText.text = $"Generation: {currentGeneration}\n" +
            $"Best Last Gen: {highestFitnessThisGen}";
    }
}
