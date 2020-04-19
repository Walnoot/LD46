using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeDialog : MonoBehaviour {
    public List<UpgradeTree> UpgradeTrees;

    public GameObject UpgradeTreeLabel, UpgradeTreeLayout, UpgradeButton;

    private CarController car;
    
    private List<GameObject> uiElements = new List<GameObject>();
    
    void Start() {
        car = FindObjectOfType<CarController>();

        AddUI();
    }

    private void AddUI() {
        var layout = GetComponentInChildren<VerticalLayoutGroup>().gameObject;
        
        foreach (UpgradeTree tree in UpgradeTrees) {
            var label = Instantiate(UpgradeTreeLabel, layout.transform);
            label.GetComponent<Text>().text = tree.name;
            uiElements.Add(label);

            var group = Instantiate(UpgradeTreeLayout, layout.transform);
            uiElements.Add(group);

            for (var i = 0; i < tree.Upgrades.Count; i++) {
                Upgrade upgrade = tree.Upgrades[i];
                var button = Instantiate(UpgradeButton, @group.transform);

                int level = car.GetUpgradeLevel(tree);
                bool canBuy = i == level;

                string text = i >= level ? upgrade.name + "\n" + upgrade.Price.ToString() : upgrade.name;
                button.GetComponentInChildren<Text>().text = text;

                Button b = button.GetComponent<Button>();
                b.interactable = canBuy;

                if (canBuy) {
                    b.onClick.AddListener(() => {
                        Debug.Log("click button " + upgrade);
                        
                        if (car.BuyUpgrade(tree, upgrade)) {
                            foreach (var element in uiElements) {
                                Destroy(element);
                            }
                            AddUI();
                        }
                    });
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            gameObject.SetActive(false);
        }
    }
}
