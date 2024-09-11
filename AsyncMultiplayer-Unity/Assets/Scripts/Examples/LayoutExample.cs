using UnityEngine;
using UnityEngine.UIElements;

public class LayoutExample : MonoBehaviour
{
    private UIDocument uiDocument;
    [SerializeField] private Request request;
    private void OnEnable()
    {
        uiDocument = GetComponent<UIDocument>();
        VisualElement root = uiDocument.rootVisualElement;
        TextField name = root.Q<TextField>("FruitName");
        TextField color = root.Q<TextField>("FruitColor");
        TextField quantity = root.Q<TextField>("Quantity");
        Button submitButton = root.Q<Button>("Submit");
        submitButton.RegisterCallback<ClickEvent>(evt =>
        {
            Fruit newFruit = new Fruit
            {
                fruit_name = name.value,
                color = color.value,
                quantity = int.Parse(quantity.value)
            };
            StartCoroutine(request.RequestExample(newFruit));
        });
    }
}
