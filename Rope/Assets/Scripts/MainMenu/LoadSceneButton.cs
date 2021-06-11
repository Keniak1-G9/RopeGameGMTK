using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadSceneButton : MonoBehaviour
{
	[SerializeField]
	private string sceneName;

	void Start()
	{
		var button = this.GetComponent<Button>();
		button.onClick.AddListener(this.ButtonClicked);
	}

	private void ButtonClicked()
	{
		SceneManager.LoadScene(this.sceneName);
	}
}