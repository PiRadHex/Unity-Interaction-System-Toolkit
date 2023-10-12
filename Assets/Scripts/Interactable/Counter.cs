using UnityEngine;
using TMPro;

public class Counter : Interactable
{
    public bool reset = false;
    public int inputNumber = 1;
    public int outputNumber = 0;
    public int maximumOutput = 99;
    public bool isIncrement = true; // Variable to determine whether to increment or decrement.
    public bool useTextInput;
    public TextMeshPro input;
    public TextMeshPro output;

    public override void Start()
    {
        base.Start();
        
        if (useTextInput && input != null)
        {
            int.TryParse(input.text, out inputNumber);
        }

        if (output != null)
        {
            int.TryParse(output.text, out outputNumber);
        }
    }

    public override void Interact(Transform interactingObjectTransform)
    {
        base.Interact(interactingObjectTransform);

        if (useTextInput && input != null)
        {
            int.TryParse(input.text, out inputNumber);
        }

        if (output != null)
        {
            int.TryParse(output.text, out outputNumber);
        }

        // Increment or decrement based on isIncrement.
        if (isIncrement)
        {
            // Add the parsed input to the output.
            outputNumber += inputNumber;
        }
        else
        {
            outputNumber -= inputNumber;
        }
        

        // Clamp output between 0 and maximumOutput.
        outputNumber = Mathf.Clamp(outputNumber, 0, maximumOutput);

        if (reset)
        {
            outputNumber = 0;
        }

        // Update the output text.
        if (output != null)
        {
            output.text = outputNumber.ToString();
        }
    }
}
