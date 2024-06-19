using UnityEngine;

namespace MyScript
{
    public class AssignTexture : MonoBehaviour
    {
        public ComputeShader shader;

        public int texResolution = 256;

        private Renderer _renderer;

        /// <summary>
        /// cs写入的texture
        /// </summary>
        private RenderTexture _outputTexture;
        /// <summary>
        /// cs的Handle
        /// </summary>
        private int _kernelHandle;

        private static readonly int m_Result = Shader.PropertyToID("Result");
        private static readonly int m_MainTex = Shader.PropertyToID("_MainTex");

        void Start()
        {
            _outputTexture = new RenderTexture(texResolution, texResolution, 0);
            //准许cs写入
            _outputTexture.enableRandomWrite = true;
            _outputTexture.Create();
            
            _renderer = GetComponent<Renderer>();
            _renderer.enabled = true;
            
            InitShader();
        }


        void InitShader()
        {
            _kernelHandle = shader.FindKernel("CSMain");
            //将结果输出到纹理
            shader.SetTexture(_kernelHandle,m_Result,_outputTexture);
            //设置Render的主纹理
            _renderer.material.SetTexture(m_MainTex,_outputTexture);
            DispatchShader(texResolution/16,texResolution/16);
        }

        void DispatchShader(int x,int y)
        {
            shader.Dispatch(_kernelHandle,x,y,1);
        }
        
        
    }
}
