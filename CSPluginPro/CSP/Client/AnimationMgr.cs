using Mirror;
using System;
using UnityEngine;
using UnityEngine.UI;
using static PluginExample.AnimationMgr;

namespace PluginExample
{
    public class AnimationData<T> where T : IAnimatableXYZ
    {
        public T Target;
        public Vector3 Start;
        public Vector3 End;
        public float Duration;
        public float Elapsed;
        public float Interval;



        public AnimationData(T Target, Vector3 Start, Vector3 End, float Duration, float Interval = 0.016f, float Elapsed = 0) 
        {

            this.Target = Target;
            this.Start = Start;
            this.End = End;
            this.Duration = Duration;
            this.Interval = Interval;
            this.Elapsed = Elapsed;
        }

    }

    public class AnimationMgr : Akequ.Base.Room
    {
        private static AnimationMgr _instance;
        public static AnimationMgr instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AnimationMgr();
                    return _instance;
                }
                return _instance;
            }
        }

        public interface IAnimatableXYZ
        {
            float x { get; set; }
            float y { get; set; }
            float z { get; set; }
        }

        

        /// <summary>
        /// 基础线性动画
        /// </summary>
        /// <typeparam name="T">实现IAnimatableXYZ接口的目标类型</typeparam>
        /// <param name="target">动画目标对象</param>
        /// <param name="start">起始坐标</param>
        /// <param name="end">结束坐标</param>
        /// <param name="duration">动画时长(秒)</param>
        /// <param name="interval">更新间隔(秒，默认0.016秒≈60FPS)</param>
        public void BasicLinear<T>(T target, Vector3 start, Vector3 end, float duration, float interval = 0.016f)
            where T : IAnimatableXYZ
        {
            if (duration <= 0 || target == null) return;

            // 初始化起始位置
            target.x = start.x;
            target.y = start.y;
            target.z = start.z;

            // 动画参数封装
            var animationData = new AnimationData<T>(target, start, end, duration, 0f, interval);
            

            // 定义动画更新步骤
            Action updateStep = null;
            updateStep = () =>
            {
                animationData.Elapsed += animationData.Interval;
                float progress = Mathf.Min(animationData.Elapsed / animationData.Duration, 1f);

                // 线性插值计算当前位置
                animationData.Target.x = Mathf.Lerp(animationData.Start.x, animationData.End.x, progress);
                animationData.Target.y = Mathf.Lerp(animationData.Start.y, animationData.End.y, progress);
                animationData.Target.z = Mathf.Lerp(animationData.Start.z, animationData.End.z, progress);

                // 未完成则继续调度
                if (progress < 1f)
                {
                    Invoke(updateStep, animationData.Interval);
                }
                else
                {
                    // 确保最终位置精确到位
                    animationData.Target.x = animationData.End.x;
                    animationData.Target.y = animationData.End.y;
                    animationData.Target.z = animationData.End.z;
                }
            };

            // 启动第一次更新
            Invoke(updateStep, interval);
        }

        /// <summary>
        /// 使用动画曲线实现非线性动画
        /// </summary>
        /// <typeparam name="T">实现IAnimatableXYZ接口的目标类型</typeparam>
        /// <param name="target">动画目标对象</param>
        /// <param name="start">起始位置</param>
        /// <param name="end">结束位置</param>
        /// <param name="duration">动画持续时间(秒)</param>
        /// <param name="curve">动画曲线(控制缓动效果)</param>
        /// <param name="interval">更新间隔(秒)，默认0.02秒</param>
        public void BasicCurve<T>(T target, Vector3 start, Vector3 end, float duration, AnimationCurve curve, float interval = 0.02f)
            where T : IAnimatableXYZ
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (curve == null) throw new ArgumentNullException(nameof(curve));
            if (duration <= 0) throw new ArgumentException("持续时间必须大于0", nameof(duration));
            if (interval <= 0) interval = 0.02f;

            // 使用数组包装避免值类型捕获问题
            var animationDataWrapper = new[] { new AnimationData<T>(
                Elapsed: 0,
                Interval: interval,
                Target: target,
                Start: start,
                End: end,
                Duration: duration
            ) };

            Action updateStep = null;
            updateStep = () =>
            {
                animationDataWrapper[0] = new AnimationData<T>(
                    Elapsed: animationDataWrapper[0].Elapsed + animationDataWrapper[0].Interval,
                    Interval: animationDataWrapper[0].Interval,
                    Target: animationDataWrapper[0].Target,
                    Start: animationDataWrapper[0].Start,
                    End: animationDataWrapper[0].End,
                    Duration: animationDataWrapper[0].Duration
                );

                var progress = Mathf.Min(animationDataWrapper[0].Elapsed / animationDataWrapper[0].Duration, 1f);
                var curveProgress = curve.Evaluate(progress); // 应用曲线缓动

                // 计算新位置并更新目标
                var newTarget = animationDataWrapper[0].Target;
                newTarget.x = Mathf.Lerp(animationDataWrapper[0].Start.x, animationDataWrapper[0].End.x, curveProgress);
                newTarget.y = Mathf.Lerp(animationDataWrapper[0].Start.y, animationDataWrapper[0].End.y, curveProgress);
                newTarget.z = Mathf.Lerp(animationDataWrapper[0].Start.z, animationDataWrapper[0].End.z, curveProgress);
                animationDataWrapper[0].Target = newTarget;

                // 未完成则继续调度
                if (progress < 1f)
                {
                    Invoke(updateStep, animationDataWrapper[0].Interval);
                }
                else
                {
                    // 确保最终位置精确到位
                    var finalTarget = animationDataWrapper[0].Target;
                    finalTarget.x = animationDataWrapper[0].End.x;
                    finalTarget.y = animationDataWrapper[0].End.y;
                    finalTarget.z = animationDataWrapper[0].End.z;
                    animationDataWrapper[0].Target = finalTarget;
                }
            };

            // 启动第一次更新
            Invoke(updateStep, interval);
        }

        /// <summary>
        /// 使用打印机动画输出文本
        /// </summary>
        /// <param name="gameObject">文本对应的游戏对象（确保你的Text组件在同一级）</param>
        /// <param name="text">输出的文本 </param>
        /// <param name="v">速度（匀速）</param>
        public void PrinterText(GameObject gameObject, string txt, float v, int index = 1)
        {
            if (gameObject.TryGetComponent<Text>(out Text text))
            {
                Action updateStep = null;
                updateStep = () => 
                {
                    text.text = txt.Substring(0, index);
                    if (index != txt.Length)
                    {
                        index += 1;
                        Invoke(updateStep, v);
                    }
                };
            }
        }
    }
}