using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour
{
  public void choiMoi()
    {
        SceneManager.LoadScene(1);
    }
   public void thoat()
    {
        Application.Quit();
    }
}

