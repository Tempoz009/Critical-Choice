using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightObject : MonoBehaviour
{
    public Material highlightMaterial; // Материал с эффектом подсветки или контура
    private Material originalMaterial; // Исходный материал объекта
    private Renderer objectRenderer; // Ссылка на компонент Renderer объекта

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalMaterial = objectRenderer.material;
    }

    void OnMouseEnter()
    {
        // При наведении курсора на объект меняем материал на материал с эффектом подсветки
        objectRenderer.material = highlightMaterial;
    }

    void OnMouseExit()
    {
        // При выходе курсора из зоны объекта возвращаем исходный материал
        objectRenderer.material = originalMaterial;
    }
}
