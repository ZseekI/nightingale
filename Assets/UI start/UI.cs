using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Ui : MonoBehaviour
{
  public void start_game ()
  {
	  SceneManager.LoadScene(1);
  }

   public void exit_game ()
  {
	 Application.Quit();
  }
}
