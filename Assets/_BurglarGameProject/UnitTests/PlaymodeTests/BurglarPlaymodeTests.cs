using System;
using System.Collections;
using System.Linq;
using _BurglarGameProject.Scripts;
using _BurglarGameProject.Scripts.Settings;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace _BurglarGameProject.UnitTests.PlaymodeTests
{
    public class BurglarPlaymodeTests
    {
        private const int BurglarGameSceneIndex = 0;

        private IBurglarGameSettings _gameSettings;
        
        [SetUp]
        public void SetUp()
        {
            if (SceneManager.GetActiveScene().buildIndex == BurglarGameSceneIndex)
            {
                return;
            }
            
            SceneManager.LoadScene(BurglarGameSceneIndex);
        }
        
        [TearDown]
        public void TearDown()
        {
        }

        [UnityTest]
        public IEnumerator IsMainSceneConfigured()
        {
            while (SceneManager.GetActiveScene().buildIndex != BurglarGameSceneIndex)
            {
                yield return null;
            }

            BurglarGameCompositionRoot mainCompositionRoot = 
                GameObject.FindObjectOfType<BurglarGameCompositionRoot>();
            
            Assert.IsNotNull(mainCompositionRoot);

            _gameSettings = mainCompositionRoot.GameSettings;

            Assert.IsNotNull(_gameSettings);   
        }
    }
}
