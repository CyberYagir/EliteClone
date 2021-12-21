using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TODO : MonoBehaviour
{
    [SerializeField] [TextArea(10, int.MaxValue)] private string text;
}
