using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Skillhub.Tasks.CloudSave
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private TMP_Text _valuesAmount;
        [SerializeField] private AudioClip _audio;
        [SerializeField] private Transform[] _effects;
        private int _values;

        private void Update()
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                AddValue();
                AudioController.Instance.PlayOneShotSFXSound(_audio);
            }
        }

        private void AddValue()
        {
            _values += 1;
            UpdateText();

            Transform obj = _effects[Random.Range(0, _effects.Length)];
            obj.gameObject.SetActive(true);
            _ = obj.DOLocalMoveY(2f, 1f).SetEase(Ease.InOutQuad).OnComplete(() => obj.gameObject.SetActive(false));
        }

        private void UpdateText()
        {
            _valuesAmount.text = _values.ToString();
        }

        public int GetValues()
        {
            return _values;
        }

        public void SetValues(int amount)
        {
            _values = amount;
            UpdateText();
        }
    }
}
