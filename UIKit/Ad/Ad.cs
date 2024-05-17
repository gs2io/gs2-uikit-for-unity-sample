using System.Collections;
using Gs2.Gs2Auth.Model;
using Gs2.Unity.Util;
using UnityEngine;

#if GS2_ENABLE_ADMOB
using GoogleMobileAds.Api;
#endif

public class Ad : MonoBehaviour
{
    void Start()
    {
        IEnumerator Impl() {
#if GS2_ENABLE_ADMOB
            {
                var future = AdMobUtil.InitializeFuture(
                    new RequestConfiguration() {
                        TestDeviceIds = new List<string> {
                            "4cd8a25ecc6250e3c140e365e5a543ff",
                        },
                    }
                );
                yield return future;
                if (future.Error != null) {
                    foreach (var e in future.Error.errors) {
                        Debug.Log(e.Message);
                    }
                }
            }
#endif
#if GS2_ENABLE_UNITY_ADS
            {
                var future = UnityAdUtil.InitializeFuture(
                    "5416096"
                );
                yield return future;
            }
#endif
            yield return null;
        }
        StartCoroutine(Impl());
    }

    public void StartAdMob() {
#if GS2_ENABLE_ADMOB
        IEnumerator Impl() {
            {
                var future = AdMobUtil.ViewFuture(
                    "ca-app-pub-8090851552121537/9708453802",
                    new GameSession(new AccessToken {
                        UserId = "user-0001",
                    })
                );
                yield return future;
                if (future.Error != null) {
                    foreach (var e in future.Error.errors) {
                        Debug.Log(e.Message);
                    }
                }
            }
        }
        StartCoroutine(Impl());
#endif
    }

    public void StartUnityAd() {
#if GS2_ENABLE_UNITY_ADS
        IEnumerator Impl() {
            {
                var future = UnityAdUtil.ViewFuture(
                    "test",
                    new GameSession(
                        null,
                        null,
                        null,
                        null
                    ) {
                        AccessToken = new AccessToken {
                            UserId = "user-0001",
                        }
                    });
                yield return future;
            }
        }
        StartCoroutine(Impl());
#endif
    }
    
    public void StartAppLovinMax() {
#if GS2_ENABLE_APPLOVIN_MAX
        IEnumerator Impl() {
            {
                var future = AppLovinMaxUtil.ViewFuture(
                    "vrzqNRVuqwZNpmssqTUFWH_D4dR1LyDmFrCVzTyQU5w8CAV5QTehTpbCZoRN40aHHQunA9osWdWg2eVSKpEELu",
                    "e2e9126da8884030",
                    new GameSession(
                        null,
                        null,
                        null,
                        null
                    ) {
                        AccessToken = new AccessToken {
                            UserId = "user-0001",
                        }
                    });
                yield return future;
            }
        }
        StartCoroutine(Impl());
#endif
    }
}
