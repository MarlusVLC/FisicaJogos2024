using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _2._RevisaoVetores
{
    public class SierpinskyUI : MonoBehaviour
    {
        [SerializeField] private SierpinskiTriangle sierpinskiTriangle;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button confirmationButton;

        private void Start()
        {
            inputField.onSubmit.AddListener(ValidateSierpinskyValue);
            confirmationButton.onClick.AddListener(() => inputField.onSubmit.Invoke(inputField.text));
        }

        private void OnDisable()
        {
            inputField.onSubmit.RemoveAllListeners();
            confirmationButton.onClick.RemoveAllListeners();
        }

        private void ValidateSierpinskyValue(string input)
        {
            var value = Convert.ToInt32(input);
            sierpinskiTriangle.DrawSierpinsky(value);
        }
    }
}