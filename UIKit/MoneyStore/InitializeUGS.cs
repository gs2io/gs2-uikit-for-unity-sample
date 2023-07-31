using System;
using Unity.Services.Core;
using UnityEngine;

namespace Scenes.Samples.UIKit.MoneyStore
{
    public class InitializeUGS : MonoBehaviour
    {
        async void Start() {
            try {
                var options = new InitializationOptions();
 
                await UnityServices.InitializeAsync(options);
            }
            catch (Exception exception) {
                Debug.LogError(exception.Message);
            }
        }
    }
}
