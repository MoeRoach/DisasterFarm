// File create date:2019/9/22
// Created By Yu.Liu
namespace RoachLite.Common {
    /// <summary>
    /// 成对的数据结构
    /// </summary>
    public struct PairData {

        public string key;
        public object value;

        public PairData(string k, object v) {
            key = k;
            value = v;
        }

        public T GetValue<T>() where T : class {
            return value as T;
        }
    }

    /// <summary>
    /// 整数对
    /// </summary>
    public struct IntegerPair {
        
        public int key;
        public int value;

        public IntegerPair(int k, int v) {
            key = k;
            value = v;
        }
    }
}
