using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SlideSystem : MonoBehaviour
{
    [Header("Configuración de Diapositivas")]
    [Tooltip("Arreglo de GameObjects que representan cada diapositiva")]
    [SerializeField] private GameObject[] slides;

    [Header("Configuración de UI")]
    [Tooltip("Botón para avanzar a la siguiente diapositiva")]
    [SerializeField] private Button nextButton;
    [Tooltip("Botón para retroceder a la diapositiva anterior")]
    [SerializeField] private Button previousButton;
    [Tooltip("Botón para finalizar el tutorial (se activa al llegar a la última diapositiva)")]
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

        // Controlar visibilidad del botón Anterior
        if (currentSlideIndex > 0)
        {
            previousButton.gameObject.SetActive(true); // Activar el botón "Anterior"
        }
        else
        {
            previousButton.gameObject.SetActive(false); // Desactivar el botón "Anterior"
        }

        // Si estamos en la última diapositiva...
        if (currentSlideIndex == slides.Length - 1)
        {
            // Desactivamos el botón de avanzar y activamos el botón de finalizar
            nextButton.gameObject.SetActive(false);
            finishButton.gameObject.SetActive(true);
        }
        else
        {
            // En cualquier otra diapositiva, activamos el botón de avanzar y desactivamos el de finalizar
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
    /// Muestra la diapositiva en el índice indicado y actualiza la UI.
    /// Solo desactiva la diapositiva que se oculta y activa la nueva.
    /// </summary>
    /// <param name="index">Índice de la diapositiva a mostrar</param>
    private void ShowSlide(int index)
    {
        // Verifica que el índice sea válido
        if (index < 0 || index >= slides.Length)
        {
            return;
        }

        // Desactiva la diapositiva actual
        slides[currentSlideIndex].SetActive(false);
        // Activa la nueva diapositiva
        slides[index].SetActive(true);
        // Actualiza el índice actual
        currentSlideIndex = index;
        // Actualiza la UI (botones e indicador)
        UpdateUI();
    }

    public void FinishTutorial()
    {
        // Aquí puedes agregar lógica adicional al finalizar
        gameObject.SetActive(false);
    }
}