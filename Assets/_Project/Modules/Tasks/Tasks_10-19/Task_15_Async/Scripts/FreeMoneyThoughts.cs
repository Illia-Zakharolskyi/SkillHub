using UnityEngine;
using TMPro;
using System.Security.AccessControl;
using System.Collections;

namespace TaskAsync
{
    public class FreeMoneyThoughts : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        private bool _entered = false;
        private bool _entered2 = false;
        private float _charDelay = 0.12f;
        private float _charDelay2 = 0.08f;
        private Coroutine routine = null;

        private void OnTriggerEnter(Collider other)
        {
            if (!_entered)
            {
                _text.gameObject.SetActive(true);
                routine = StartCoroutine(TypeTextRoutine());
                _entered = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_entered2)
            {
                StopCoroutine(routine);
                _text.gameObject.SetActive(false);
                _entered2 = true;
            }
        }

        private IEnumerator TypeTextRoutine()
        {
            _text.ForceMeshUpdate();

            int totalChars = _text.textInfo.characterCount;

            for (int i = 0; i <= totalChars; i++)
            {
                _text.maxVisibleCharacters = i;
                yield return new WaitForSeconds(_charDelay);
            }

            yield return new WaitForSeconds(1f);
            routine = null;
            routine = StartCoroutine(EraseTextRoutine());
        }

        private IEnumerator EraseTextRoutine()
        {
            int currentChars = _text.maxVisibleCharacters;

            if (currentChars > _text.textInfo.characterCount || currentChars == 0)
            {
                _text.ForceMeshUpdate();
                currentChars = _text.textInfo.characterCount;
            }

            for (int i = currentChars; i >= 0; i--)
            {
                _text.maxVisibleCharacters = i;
                yield return new WaitForSeconds(_charDelay2);
            }

            _text.gameObject.SetActive(false);
        }
    }
}
