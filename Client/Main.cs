using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Main : BaseScript
    {
        public static int PlayerPedID { get => API.PlayerPedId(); }
        public Main()
        {
            API.RegisterCommand("givehorse", new Action(async () => await GiveVehicle("coach2")), false);

            Tick += OnTick;
        }

        [Tick]
        private async Task OnTick()
        {
            if (API.IsControlJustPressed(PlayerPedID, 0x24978A28))
            {
                //stfu
                //await new Task<bool>(() => true);

                await GiveVehicle("coach2");
            }
        }

        private async Task<int> GiveVehicle(string name)
        {
            uint vehicleHash = Utils.GetOrCreateCachedHash(name);

            if (!await Utils.LoadModel(vehicleHash))
            {
                return -1;
            }

            Vector3 pos = API.GetEntityCoords(PlayerPedID, false, false);
            Vector3 forwardPos = API.GetEntityForwardVector((uint)PlayerPedID);
            pos += (forwardPos * 5);
            float heading = API.GetEntityHeading(PlayerPedID);
            int vehicle = API.CreateVehicle(vehicleHash, pos.X, pos.Y, pos.Z, heading, false, false, false, false);

            return vehicle;
        }

        private async Task<int> GiveMissionVehicle(string name)
        {
            int id = await GiveVehicle(name);
            if (id != -1)
            {
                API.SetEntityAsMissionEntity(id, true, true);
            }

            return id;
        }
    }

    public class Utils
    {
        public static Map<uint, string> CachedHashes = new Map<uint, string>();

        public static uint GetOrCreateCachedHash(string name)
        {
            if (CachedHashes.Reverse.Contains(name))
            {
                return CachedHashes.Reverse[name];
            }
            else
            {
                uint hash = GetHash(name);
                CachedHashes.Add(hash, name);

                return hash;
            }
        }

        public static async Task<bool> LoadModel(uint hash)
        {
            if (API.IsModelValid(hash))
            {
                API.RequestModel(hash, false);
                while (!API.HasModelLoaded(hash))
                {
                    Debug.WriteLine($"Waiting for model {hash} to load...");

                    //wait 100 ms
                    await BaseScript.Delay(100);
                }
                return true;
            }
            else
            {
                Debug.WriteLine($"Model {hash} is not valid!");
                return false;
            }
        }

        public static uint GetHash(string name)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(name.ToLowerInvariant());

            uint num = 0u;
            for (int i = 0; i < bytes.Length; i++)
            {
                num += (uint)bytes[i];
                num += num << 10;
                num ^= num >> 6;
            }
            num += num << 3;
            num ^= num >> 11;

            return num + (num << 15);
        }
    }

    public class Map<T1, T2> : IEnumerable<KeyValuePair<T1, T2>>
    {
        private readonly Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();
        private readonly Dictionary<T2, T1> _reverse = new Dictionary<T2, T1>();

        public Map()
        {
            Forward = new Indexer<T1, T2>(_forward);
            Reverse = new Indexer<T2, T1>(_reverse);
        }

        public Indexer<T1, T2> Forward { get; private set; }
        public Indexer<T2, T1> Reverse { get; private set; }

        public void Add(T1 t1, T2 t2)
        {
            _forward.Add(t1, t2);
            _reverse.Add(t2, t1);
        }

        public void Remove(T1 t1)
        {
            T2 revKey = Forward[t1];
            _forward.Remove(t1);
            _reverse.Remove(revKey);
        }

        public void Remove(T2 t2)
        {
            T1 forwardKey = Reverse[t2];
            _reverse.Remove(t2);
            _forward.Remove(forwardKey);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
        {
            return _forward.GetEnumerator();
        }

        public class Indexer<T3, T4>
        {
            private readonly Dictionary<T3, T4> _dictionary;

            public Indexer(Dictionary<T3, T4> dictionary)
            {
                _dictionary = dictionary;
            }

            public T4 this[T3 index]
            {
                get { return _dictionary[index]; }
                set { _dictionary[index] = value; }
            }

            public bool Contains(T3 key)
            {
                return _dictionary.ContainsKey(key);
            }
        }
    }
}
