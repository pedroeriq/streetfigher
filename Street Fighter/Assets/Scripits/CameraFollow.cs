using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player1; // Referência ao jogador 1
    public Transform player2; // Referência ao jogador 2
    public float smoothSpeed = 0.5f; // Velocidade de movimento da câmera
    public float minXLimit; // Limite mínimo no eixo X
    public float maxXLimit; // Limite máximo no eixo X
    public float minYLimit; // Limite mínimo no eixo Y
    public float maxYLimit; // Limite máximo no eixo Y

    private Vector3 initialCameraPosition; // Posição inicial da câmera

    void Start()
    {
        // Salvar a posição inicial da câmera
        initialCameraPosition = transform.position;
    }

    void FixedUpdate()
    {
        // Encontrar o ponto médio entre os dois jogadores
        Vector3 midpoint = (player1.position + player2.position) / 2.0f;

        // Garantir que a câmera não ultrapasse os limites da fase
        midpoint.x = Mathf.Clamp(midpoint.x, minXLimit, maxXLimit);
        midpoint.y = Mathf.Clamp(midpoint.y, minYLimit, maxYLimit);

        // Definir a posição da câmera com uma suavização
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, midpoint + initialCameraPosition, smoothSpeed * Time.fixedDeltaTime);
        transform.position = smoothedPosition;
    }
}