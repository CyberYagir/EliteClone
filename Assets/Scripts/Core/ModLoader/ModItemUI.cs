using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModItemUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text modName, modDesc;
    [SerializeField] private RawImage icon;
    [SerializeField] private List<Button> buttons;
    private Mod mod;
    public void Init(Mod mod)
    {
        this.mod = mod;
        SetData(this.mod);
    }

    public string GetNameModName(int modID)
    {
        return Path.GetFileNameWithoutExtension(ModsManager.Instance.modLoader.loadChain.mods[modID]);
    }
    public void SetData(Mod mod)
    {
        if (ModsManager.Instance.modLoader.mods.Count == 1)
        {
            buttons.ForEach(x=>x.gameObject.SetActive(false));
        }
        else
        {
            if (GetNameModName(0) == mod.data.modName)
            {
                buttons.First().gameObject.SetActive(false);
            }

            if (GetNameModName(ModsManager.Instance.modLoader.mods.Count - 1) == mod.data.modName)
            {
                buttons.Last().gameObject.SetActive(false);
            }
        }
        
        if (mod == null || mod.data == null){Destroy(gameObject); return;}

        modName.text = mod.data.modName + "[" + mod.data.modVersionData.ToString() + "]";
        modDesc.text = mod.data.modDescription;
        
        if (mod.data.iconData.textureRaw.Length != 0)
        {
            icon.texture = mod.data.iconData.GetTexture();
        }
    }
    
    
    public void OnPointerClick(PointerEventData eventData)
    {
        //mod.LoadSceneFromAsset(0);
    }

    public void MoveUp()
    {
        ModsManager.Instance.modLoader.loadChain.MoveModUp(mod.data.modName);
    }
    public void MoveDown()
    {
        ModsManager.Instance.modLoader.loadChain.MoveModDown(mod.data.modName);
    }
}
