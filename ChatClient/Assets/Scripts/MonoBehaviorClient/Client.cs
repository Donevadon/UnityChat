using TMPro;
using UnityEngine;
using Zenject;

namespace MonoBehaviorClient
{
    public class Client : MonoBehaviour
    {
        [SerializeField] private TMP_InputField input;
        private IConnection _connection;

        private void Start()
        {
            _connection.Connect();
        }

        private void OnDestroy()
        {
            _connection?.Dispose();
        }

        [Inject]
        private void Init(IConnection connection)
        {
            _connection = connection;
        }

        public void Send()
        {
            _connection.SendMessage(input.text);
            input.text = null;
        }
    }
}