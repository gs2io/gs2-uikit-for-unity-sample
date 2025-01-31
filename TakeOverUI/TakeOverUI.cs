using System;
using System.Collections.Generic;
using System.Linq;
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#endif
using Gs2.Unity.Core;
using Gs2.Unity.Util;
using UnityEngine;

namespace Scenes.Samples.Simple.TakeOverUI
{
    public class TakeOverUI : MonoBehaviour
    {
#if GS2_ENABLE_UNITASK && GS2_ENABLE_GREE_WEBVIEW
        public WebViewObject webViewObject;
        
        public static async UniTask<string> OpenAuthentication(
            WebViewObject webView,
            Gs2Domain gs2,
            string namespaceName,
            IGameSession gameSession,
            int type
        ) {
            string idToken = null;
            webView.Init(
                separated: true,
                ld: url =>
                {
                    Debug.Log(url);
                    if (new Uri(url).LocalPath.EndsWith("/done")) {
                        var codeField = new Uri(url).Query.Replace("?", "").Split("&").Select(v => new KeyValuePair<string,string>(v[..v.IndexOf("=", StringComparison.Ordinal)], v[(v.IndexOf("=", StringComparison.Ordinal)+1)..])).FirstOrDefault(v => v.Key == "id_token");
                        idToken = Uri.UnescapeDataString(codeField.Value);
                        webView.SetVisibility(false);
                    }
                }
            );
            webView.LoadURL(
                (await gs2.Account.Namespace(
                    namespaceName
                ).Me(
                    gameSession
                ).GetAuthorizationUrlAsync(
                    type
                )).AuthorizationUrl
            );
            webView.SetInteractionEnabled(true);
            webView.SetVisibility(true);

            await UniTask.WaitWhile(() => idToken == null);

            return idToken;
        }
        
        public void Start() {
            var namespaceName = "test";
            var type = 0;
            
            async UniTask Impl() {
                var idToken = await OpenAuthentication(
                    webViewObject,
                    Gs2ClientHolder.Instance.Gs2,
                    namespaceName,
                    Gs2GameSessionHolder.Instance.GameSession,
                    type
                );
                Debug.Log(idToken);

                var takeOver = await (await Gs2ClientHolder.Instance.Gs2.Account.Namespace(
                    namespaceName
                ).Me(
                    Gs2GameSessionHolder.Instance.GameSession
                ).TakeOver(
                    type
                ).AddTakeOverSettingOpenIdConnectAsync(
                    idToken
                )).ModelAsync();
                
                Debug.Log(takeOver.UserId);
                Debug.Log(takeOver.UserIdentifier);
            }

            StartCoroutine(Impl().ToCoroutine());
        }
#endif
    }
}
