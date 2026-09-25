using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class FirmasDigitales
    {
        public FirmasDigitales()
        {
            PublicKey = "-----BEGIN PUBLIC KEY-----\r\nMIIBojANBgkqhkiG9w0BAQEFAAOCAY8AMIIBigKCAYEA3fFowWEro2U8PYfT9aUL\r\nt7dkxq3oj/PtkIOEP8eCSmLAShVuXSeYbAj2zy7CDGSRjKAsCnkVeqBks5k0UNx0\r\nL6reyceTQVr2TJxh7TsJlN5+m04GiPmWxp+EfMb+sL935GSfAGRjm82i2DN+tcEd\r\n52B1FLcDzRnbcFbNFJq+gwVTexueNLgi5WNHKZ5WZltFxJcoPX2vBv98B3y05k5j\r\neOXtK3p3Mh9gJ85+UOZ7/RcONqZiCQdZfnrBGRP/3ThUZjVC+RCpbUIGwiUNLFaE\r\nxvgBgWHliDS8IMsAegUnhmFNFXTkh68c6CSxrSEO9/UZMNhoctcLKZrEy0Z3VOug\r\neYkHkYWBinXRIiw3hw82K0nq5aGwR3LEJePWsaqkJZRvRAUMNmH/lMfw5ia6ITN2\r\ndxMrjsyF62XF/0ancKEumLu5DPoPI+IkodbaKDdpt/57JF/2GOLrGcmqBcIzx0Ve\r\nxiTXLccRUvfCqaGix5f9iplrHkpOzYNhK6TvLvv3sIIpAgMBAAE=\r\n-----END PUBLIC KEY-----";
        }

        public string PublicKey { get; set;  }
    }
}
