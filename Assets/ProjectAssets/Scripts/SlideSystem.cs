using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SlideSystem : MonoBehaviour
{
    [Header("Configuraci�n de Diapositivas")]
    [Tooltip("Arreglo de GameObjects que representan cada diapositiva")]
    [SerializeField] private GameObject[] slides;

    [Header("Configuraci�n de UI")]
    [Tooltip("Bot�n para avanzar a la siguiente diapositiva")]
    [SerializeField] private Button nextButton;
    [Tooltip("Bot�n para retroceder a la diapositiva anterior")]
    [SerializeField] private Button previousButton;
    [Tooltip("Bot�n para finalizar el tutorial (se activa al llegar a la �ltima diapositiva)")]
    [SerializeField] private Button finishButton;
    [Tooltip("Texto con el formato 'actual/total'")]
    [SerializeField] private TMP_Text slideCounter;

    private int currentSlideIndex = 0;

    private void Start()
    {
        InitializeSlides();
        UpdateUI();
    }

    private void InitializeSlides()
    {
        // Verifica que se haya asignado el arreglo de diapositivas
        if (slides == null || slides.Length == 0)
        {
            Debug.LogWarning("No se han asignado diapositivas en el SlideSystem.");
        }
        else
        {
            for (int i = 0; i < slides.Length; ++i)
            {
                slides[i].SetActive(false);
            }
            slides[currentSlideIndex].SetActive(true);
        }
    }

    private void UpdateUI()
    {
        // Actualizar contador de diapositivas
        slideCounter.text = (currentSlideIndex + 1) + "/" + slides.Length;

        // Controlar visibilidad del bot�n Anterior
        if (currentSlideIndex > 0)
        {
            previousButton.gameObject.SetActive(true); // Activar el bot�n "Anterior"
        }
        else
        {
            previousButton.gameObject.SetActive(false); // Desactivar el bot�n "Anterior"
        }

        // Si estamos en la �ltima diapositiva...
        if (currentSlideIndex == slides.Length - 1)
        {
            // Desactivamos el bot�n de avanzar y activamos el bot�n de finalizar
            nextButton.gameObject.SetActive(false);
            finishButton.gameObject.SetActive(true);
        }
        else
        {
            // En cualquier otra diapositiva, activamos el bot�n de avanzar y desactivamos el de finalizar
            nextButton.gameObject.SetActive(true);
            finishButton.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Pasa a la siguiente diapositiva si existe.
    /// </summary>
    public void NextSlide()
    {
        if (currentSlideIndex < slides.Length - 1)
        {
            ShowSlide(currentSlideIndex + 1);
        }
    }

    /// <summary>
    /// Retrocede a la diapositiva anterior si existe.
    /// </summary>
    public void PreviousSlide()
    {
        if (currentSlideIndex > 0)
        {
            ShowSlide(currentSlideIndex - 1);
        }
    }

    /// <summary>
    /// Muestra la diapositiva en el �ndice indicado y actualiza la UI.
    /// Solo desactiva la diapositiva que se oculta y activa la nueva.
    /// </summary>
    /// <param name="index">�ndice de la diapositiva a mostrar</param>
    private void ShowSlide(int index)
    {
        // Verifica que el �ndice sea v�lido
        if (index < 0 || index >= slides.Length)
        {
            return;
        }

        // Desactiva la diapositiva actual
        slides[currentSlideIndex].SetActive(false);
        // Activa la nueva diapositiva
        slides[index].SetActive(true);
        // Actualiza el �ndice actual
        currentSlideIndex = index;
        // Actualiza la UI (botones e indicador)
        UpdateUI();
    }

    public void FinishTutorial()
    {
        // Aqu� puedes agregar l�gica adicional al finalizar
        gameObject.SetActive(false);
    }
}