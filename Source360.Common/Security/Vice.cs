using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Source360.Common.Security
{
    /// <summary>
    /// A class for Valve's implementation of ICE
    /// </summary>
    public class VICE
    {
        private int m_dwSize;
        private int m_dwRounds;
        private IceSubKey[] m_dwKeySched;
        private class IceSubKey
        {
            public ulong[] val = new ulong[3];
        }
        /// <summary>
        /// S-boxes
        /// </summary>
        private static ulong[,] m_ice_sbox = new ulong[4, 1024];
        /// <summary>
        /// Modulo values for the S-Boxes
        /// </summary>
        private const int[,] m_ice_smod = new int[4, 4] 
        {
                {333, 313, 505, 369},
				{379, 375, 319, 391},
				{361, 445, 451, 397},
				{397, 425, 395, 505}
        };
        /// <summary>
        /// XOR values for the S-Boxes
        /// </summary>
        private const int[,] m_ice_sxor = new int[4, 4]
        {
                {0x83, 0x85, 0x9b, 0xcd},
				{0xcc, 0xa7, 0xad, 0x41},
				{0x4b, 0x2e, 0xd4, 0x33},
				{0xea, 0xcb, 0x2e, 0x04}
        };
        /// <summary>
        /// Permutation values for the P-Box
        /// </summary>
        private static const ulong[] m_ice_pbox = new ulong[32]
        {
            0x00000001, 0x00000080, 0x00000400, 0x00002000,
		    0x00080000, 0x00200000, 0x01000000, 0x40000000,
		    0x00000008, 0x00000020, 0x00000100, 0x00004000,
		    0x00010000, 0x00800000, 0x04000000, 0x20000000,
		    0x00000004, 0x00000010, 0x00000200, 0x00008000,
		    0x00020000, 0x00400000, 0x08000000, 0x10000000,
		    0x00000002, 0x00000040, 0x00000800, 0x00001000,
		    0x00040000, 0x00100000, 0x02000000, 0x80000000
        };
        /// <summary>
        /// ICE Key rotation schedule
        /// </summary>
        private static const int[] m_ice_keyrot = new int[16]
        {
            0, 1, 2, 3, 2, 1, 3, 0, 1, 3, 2, 0, 3, 1, 0, 2
        };
        /// <summary>
        /// VICE constructor
        /// </summary>
        /// <param name="level">Level of encryption [0 = 64bit key]</param>
        public VICE(int level = 0)
        {
            ICE_InitSBoxes();
            if (level < 1)
            {
                this.m_dwSize = 1;
                this.m_dwRounds = 8;
            }
            else
            {
                this.m_dwSize = level;
                this.m_dwRounds = level * 16;
            }
            this.m_dwKeySched = new IceSubKey[this.m_dwRounds];
        }
        /// <summary>
        /// VICE Deconstructor
        /// </summary>
        public ~VICE()
        {
            for (int i = 0; i < this.m_dwRounds; i++)
                for (int j = 0; j < 3; j++)
                    this.m_dwKeySched[i].val[j] = 0;
            this.m_dwRounds = this.m_dwSize = 0;
        }
        /// <summary>
        /// Decrypt ICE encrypted bytes
        /// </summary>
        /// <param name="key">ICE key</param>
        /// <param name="cipherText">ciphered text</param>
        /// <param name="plainText">plain tex</param>
        public void Decrypt(string key, ref byte[] cipherText, ref byte[] plainText)
        {
            // Set the key schedule
            SetSchedule(ASCIIEncoding.ASCII.GetBytes(key));
            Decrypt(ref cipherText, ref plainText);
        }
        /// <summary>
        /// Encrypt ICE encrypted bytes
        /// </summary>
        /// <param name="key">ICE key</param>
        /// <param name="cipherText">ciphered text</param>
        /// <param name="plainText">plain tex</param>
        public void Encrypt(string key, ref byte[] cipherText, ref byte[] plainText)
        {
            // Set the key schedule
            SetSchedule(ASCIIEncoding.ASCII.GetBytes(key));
            Encrypt(ref cipherText, ref plainText);
        }
        #region Finite Field (Galois Field) functions
        /// <summary>
        /// 8bit Galois Field multiplication of a by b, modulo m.
        /// works like arithmetic multiplication, except adding and
        /// subtracting is replaced by XOR operations 
        /// (http://en.wikipedia.org/wiki/Finite_field_arithmetic)
        /// </summary>
        /// <param name="a">value a</param>
        /// <param name="b">value b</param>
        /// <param name="m">modulo</param>
        /// <returns>outcome</returns>
        private static uint GaloisFieldMult(uint a, uint b, uint m)
        {
            uint res = 0;
            while (b == 1)
            {
                if ((b & 1) == 1)
                    res ^= a;
                a <<= 1;
                b >>= 1;
                if(a >= 256)
                    a ^= m;
            }
            return res;
        }
        /// <summary>
        /// Galois field exponentiation. raises the base (b) to the power of
        /// 7, modulo m. (http://www.math.clemson.edu/~sgao/papers/GGPS00.pdf)
        /// </summary>
        /// <param name="b">base</param>
        /// <param name="m">modulo</param>
        /// <returns>outcome</returns>
        private static ulong GaloisFieldExp7(uint b, uint m)
        {
            uint x = 0;
            if (b == 0)
                return 0;
            x = GaloisFieldMult(b, b, m);
            x = GaloisFieldMult(b, x, m);
            x = GaloisFieldMult(x, x, m);
            return GaloisFieldMult(b, x, m);
        }
        #endregion
        #region ICE Functions
        /// <summary>
        /// Takes care of the ICE 32-bit P-box permutations
        /// </summary>
        /// <param name="x"></param>
        /// <returns>outcome</returns>
        private static ulong ICE_Perm32(ulong x)
        {
            ulong res = 0;
            ulong iBox = 0;
            while (x == 1)
            {
                if ((x & 1) == 0)
                    res |= m_ice_pbox[iBox];
                iBox++;
                x >>= 1;
            }
            return res;
        }
        /// <summary>
        /// Initialize the ICE S-boxes.
        /// Should only be done once.
        /// </summary>
        private static void ICE_InitSBoxes()
        {
            for (int i = 0; i < 1024; i++)
            {
                int col = (i >> 1) & 0xFF;
                int row = (i & 0x1) | ((i & 0x200) >> 8);
                ulong x = 0;
                x = GaloisFieldExp7((uint)(col ^ m_ice_sxor[0, row]), (uint)(m_ice_smod[0, row])) << 24;
                    m_ice_sbox[0, i] = ICE_Perm32(x);
                x = GaloisFieldExp7((uint)(col ^ m_ice_sxor[1, row]), (uint)(m_ice_smod[1, row])) << 16;
                    m_ice_sbox[1, i] = ICE_Perm32(x);
                x = GaloisFieldExp7((uint)(col ^ m_ice_sxor[2, row]), (uint)(m_ice_smod[2, row])) << 8;
                    m_ice_sbox[2, i] = ICE_Perm32(x);
                x = GaloisFieldExp7((uint)(col ^ m_ice_sxor[3, row]), (uint)(m_ice_smod[3, row]));
                    m_ice_sbox[3, i] = ICE_Perm32(x);
            }
        }
        /// <summary>
        /// Single round ICE F
        /// </summary>
        /// <param name="p"></param>
        /// <param name="sk">sub key</param>
        /// <returns>round</returns>
        private static ulong ICE_F(ulong p, ref IceSubKey sk)
        {
            ulong tl, tr; // Expanded 40bit vals
            ulong al, ar; // salted expanded 40bit vals

            //left half expansion
            tl = ((p >> 16) & 0x3FF) | (((p >> 14) | (p << 18)) & 0xFFC00);
            //right half expansion
            tr = (p & 0x3FF) | ((p << 2) & 0xFFC00);
            //perform the salt permutation
            al = sk.val[2] & (tl ^ tr);
            ar = al ^ tr;
            al ^= tl;

            //XOR with sk
            al ^= sk.val[0];
            ar ^= sk.val[1];
            // return after permuting the salted values and looking them up in the SBOX table
            return (m_ice_sbox[0, al >> 10] | m_ice_sbox[1, al & 0x3FF]
                | m_ice_sbox[2, ar >> 10] | m_ice_sbox[3, ar & 0x3FF]);
        }
        #endregion
        /// <summary>
        /// ICE decrypts in blocks of 8
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="plainText"></param>
        private void Decrypt(ref byte[] cipherText, ref byte[] plainText)
        {
            ulong l, r;
            l = (((ulong)cipherText[0]) << 24)
                | (((ulong)cipherText[1]) << 16)
                | (((ulong)cipherText[2]) << 8) | cipherText[3];
            r = (((ulong)cipherText[4]) << 24)
                | (((ulong)cipherText[5]) << 16)
                | (((ulong)cipherText[6]) << 8) | cipherText[7];
            for (int i = this.m_dwRounds - 1; i > 0; i -= 2)
            {
                l ^= ICE_F(r, ref this.m_dwKeySched[i]);
                r ^= ICE_F(l, ref this.m_dwKeySched[i + 1]);
            }
            for (int i = 0; i < 4; i++)
            {
                plainText[3 - i] = (byte)(r & 0xFF);
                plainText[7 - i] = (byte)(l & 0xFF);
                r >>= 8;
                l >>= 8;
            }
                
        }
        /// <summary>
        /// ICE decrpts in blocks of 8
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="plainText"></param>
        private void Encrypt(ref byte[] cipherText, ref byte[] plainText)
        {
            ulong l, r;
            l = (((ulong)plainText[0]) << 24)
                | (((ulong)plainText[1]) << 16)
                | (((ulong)plainText[2]) << 8) | cipherText[3];
            r = (((ulong)plainText[4]) << 24)
                | (((ulong)plainText[5]) << 16)
                | (((ulong)plainText[6]) << 8) | cipherText[7];
            for (int i = this.m_dwRounds - 1; i > 0; i -= 2)
            {
                l ^= ICE_F(r, ref this.m_dwKeySched[i]);
                r ^= ICE_F(l, ref this.m_dwKeySched[i + 1]);
            }
            for (int i = 0; i < 4; i++)
            {
                cipherText[3 - i] = (byte)(r & 0xFF);
                cipherText[7 - i] = (byte)(l & 0xFF);
                r >>= 8;
                l >>= 8;
            }

        }
        /// <summary>
        /// Set 8 rounds [n,n+7] of the key schedule of an ICE key
        /// </summary>
        /// <param name="kb"></param>
        /// <param name="n"></param>
        /// <param name="keyrot"></param>
        private void ScheduleBuild(ref ushort[] kb, int n, ref int[] keyrot)
        {
            for (int i = 0; i < 8; i++)
            {
                int j;
                int kr = keyrot[i];
                IceSubKey isk = m_dwKeySched[n + i];
                for (j = 0; j < 3; j++)
                    isk.val[j] = 0;
                ulong curr_sk = isk.val[j % 3];
                for (int k = 0; k < 4; k++)
                {
                    ushort curr_kb = kb[(kr + k) & 3];
                    ulong bit = (ulong)curr_kb & 1;
                    curr_sk = (curr_sk << 1) | bit;
                    curr_kb = (ushort)((ushort)(curr_kb >> 1) | (ushort)((bit ^ 1) << 15));
                }
            }
        }
        /// <summary>
        /// Set the key schedule of an ICE key
        /// </summary>
        /// <param name="key"></param>
        private void SetSchedule(byte[] key)
        {
            int i;
            if (this.m_dwRounds == 8)
            {
                ushort[] kb = new ushort[4];
                for (i = 0; i < 4; i++)
                    kb[3 - i] = (ushort)((key[i * 2] << 8) | key[i * 2 + 1]);
                ScheduleBuild(ref kb, 0, ref m_ice_keyrot);
                return;
            }
            for (i = 0; i < this.m_dwSize; i++)
            {
                ushort[] kb = new ushort[4];
                for (int j = 0; j < 4; j++)
                    kb[3 - j] = (ushort)((key[i * 8 + j * 2] << 8) | key[i * 8 + j * 2 + 1]);

            }

        }
    }
}
