using System;
using System.Collections.Generic;
using System.Linq;
using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using static System.Math;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;

namespace Fusee.Tutorial.Core
{
    public class HierarchyInput : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRenderer _sceneRenderer;
        private float _camAngle = 0;
        private float _maxAngle = 1.5f;
        private float _minAngle = 0;
        private TransformComponent _baseTransform;
        private TransformComponent _bodyTransform;
        private TransformComponent _upperArmTransform;
        private TransformComponent _foreArmTransform;
        private TransformComponent _hookRightTransform;
        private TransformComponent _hookLeftTransform;
    

        SceneContainer CreateScene()
        {
            // Initialize transform components that need to be changed inside "RenderAFrame"
            _baseTransform = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 0, 0)
            };
            _bodyTransform = new TransformComponent
            {
                Rotation = new float3(0,-0.5f,0),
                Scale = new float3(1,1,1),
                Translation = new float3(0,6,0)
            };
            _upperArmTransform = new TransformComponent
            {
                Rotation = new float3(0,0,0),
                Scale = new float3(1,1,1),
                Translation = new float3(2,4,0)
            };
            _foreArmTransform = new TransformComponent
            {
                Rotation = new float3(0,0,0),
                Scale = new float3(1,1,1),
                Translation = new float3(-2,8,0)
            };
            _hookRightTransform = new TransformComponent
            {
                Rotation = new float3(0,0,0),
                Scale = new float3(1,1,1),
                Translation = new float3(-0.5f,8.5f,0)
            };
            _hookLeftTransform = new TransformComponent
            {
                Rotation = new float3(0,0,0),
                Scale = new float3(1,1,1),
                Translation = new float3(0.5f,8.5f,0)
            };

            // Setup the scene graph
            return new SceneContainer
            {
                Children = new List<SceneNodeContainer>
                {
                    // Grey base
                    new SceneNodeContainer
                    {
                        Components = new List<SceneComponentContainer>
                        {
                            // TRANSFROM COMPONENT
                            _baseTransform,

                            // SHADER EFFECT COMPONENT
                            new ShaderEffectComponent
                            {
                                Effect = SimpleMeshes.MakeShaderEffect(new float3(0.7f, 0.7f, 0.7f), new float3(0.7f, 0.7f, 0.7f), 5)
                            },

                            // MESH COMPONENT
                            SimpleMeshes.CreateCuboid(new float3(10, 2, 10))
                        }
                    },

                    //RED body
                    new SceneNodeContainer
                    {
                        Components = new List<SceneComponentContainer>
                        {
                            _bodyTransform,
                            new ShaderEffectComponent
                            {
                               Effect = SimpleMeshes.MakeShaderEffect(new float3(1, 0, 0), new float3(1, 1, 1), 5)
                            },

                                //Mesh Component
                                SimpleMeshes.CreateCuboid(new float3(2,10,2))
                            
                        },
                        Children = new List<SceneNodeContainer>
                        {
                            //Green upper arm
                            new SceneNodeContainer
                            {
                                Components = new List<SceneComponentContainer>
                                {

                                    _upperArmTransform,
                                },
                                Children = new List<SceneNodeContainer>
                                {
                                    new SceneNodeContainer
                                    {
                                        Components = new List<SceneComponentContainer>
                                        {
                                            new TransformComponent
                                            {
                                                Rotation = new float3(0,0,0),
                                                Scale = new float3(1,1,1),
                                                Translation = new float3(0,4,0)
                                            },
                                
                                            new ShaderEffectComponent
                                            {   
                                                Effect = SimpleMeshes.MakeShaderEffect(new float3(0, 1, 0), new float3(1, 1, 1), 5)
                                            },
                                            SimpleMeshes.CreateCuboid(new float3(2,10,2))
                                        }
                                    },
                                    //blue forearm
                                    new SceneNodeContainer
                                    {
                                        Components = new List<SceneComponentContainer>
                                        {
                                            _foreArmTransform,
                                        },
                                        Children = new List<SceneNodeContainer>
                                        {
                                            new SceneNodeContainer
                                            {
                                                Components = new List<SceneComponentContainer>
                                                {
                                                    new TransformComponent
                                                    {
                                                        Rotation = new float3(0,0,0),
                                                        Scale = new float3(1,1,1),
                                                        Translation = new float3(0,4,0)
                                                    },

                                                    new ShaderEffectComponent
                                                    {
                                                        Effect = SimpleMeshes.MakeShaderEffect(new float3(0,0,1), new float3(1,1,1), 5)
                                                    },
                                                    SimpleMeshes.CreateCuboid(new float3(2,10,2))
                                                }
                                            },
                                            //Hookright
                                            new SceneNodeContainer
                                            {
                                                Components = new List<SceneComponentContainer>
                                                {
                                                    _hookRightTransform,
                                                },
                                                Children = new List<SceneNodeContainer>
                                                {
                                                    new SceneNodeContainer
                                                    {
                                                        Components = new List<SceneComponentContainer>
                                                        {
                                                            new TransformComponent
                                                            {
                                                                Rotation = new float3(0,0,0),
                                                                Scale = new float3(1,1,1),
                                                                Translation = new float3(0,2.5f,0)
                                                            },

                                                            new ShaderEffectComponent
                                                            {
                                                                Effect = SimpleMeshes.MakeShaderEffect(new float3(1,1,0), new float3(1,1,1),5)
                                                            },
                                                            SimpleMeshes.CreateCuboid(new float3(1,5,1))
                                                        }
                                                    }
                                                }
                                            },

                                            //Hookleft
                                            new SceneNodeContainer
                                            {
                                                Components = new List<SceneComponentContainer>
                                                {
                                                    _hookLeftTransform,
                                                },
                                                Children = new List<SceneNodeContainer>
                                                {
                                                    new SceneNodeContainer
                                                    {
                                                        Components = new List<SceneComponentContainer>
                                                        {
                                                            new TransformComponent
                                                            {
                                                                Rotation = new float3(0,0,0),
                                                                Scale = new float3(1,1,1),
                                                                Translation = new float3(0,2.5f,0)
                                                            },

                                                            new ShaderEffectComponent
                                                            {
                                                                Effect = SimpleMeshes.MakeShaderEffect(new float3(1,1,0), new float3(1,1,1),5)
                                                            },
                                                            SimpleMeshes.CreateCuboid(new float3(1,5,1))
                                                        }
                                                    }
                                                }
                                            }



                                        }

                                    }
                                },
                            }
                        }
                    }
                }
            };
        }

        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to white (100% intensity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0.8f, 0.9f, 0.7f, 1);

            _scene = CreateScene();

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRenderer(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            //RedBody Controller
            float bodyRot = _bodyTransform.Rotation.y;
            bodyRot += 1 * Keyboard.LeftRightAxis * DeltaTime;
            _bodyTransform.Rotation = new float3(0,bodyRot,0);

            //GreenUpperArm Controller
            float upperArmGreen = _upperArmTransform.Rotation.x;
            upperArmGreen += 1*Keyboard.UpDownAxis * DeltaTime;
            _upperArmTransform.Rotation = new float3(upperArmGreen,0,0);

             //BlueforeArm Controller
            float foreArmBlue = _foreArmTransform.Rotation.x;
            foreArmBlue += 0.5f*Keyboard.UpDownAxis * DeltaTime;
            _foreArmTransform.Rotation = new float3(foreArmBlue,0,0);

            //hookRight Controller
            float angleHookRight = _hookRightTransform.Rotation.z;
            angleHookRight += 3*DeltaTime*Keyboard.ADAxis;
            if (angleHookRight > _maxAngle)
            {
                angleHookRight = _maxAngle;
            }
            if(angleHookRight < _minAngle)
            {
                angleHookRight =_minAngle;
            }
            _hookRightTransform.Rotation = new float3(0,0,angleHookRight);

            //hookLeft Controller
            float angleHookLeft = _hookLeftTransform.Rotation.z;
            angleHookLeft -= 3*DeltaTime*Keyboard.ADAxis;
            if (angleHookLeft < -_maxAngle)
            {
                angleHookLeft = -_maxAngle;
            }
            if(angleHookLeft > _minAngle)
            {
                angleHookLeft =_minAngle;
            }

            _hookLeftTransform.Rotation = new float3(0,0,angleHookLeft);


            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            // Setup the camera 
            RC.View = float4x4.CreateTranslation(0, -10, 50) * float4x4.CreateRotationY(_camAngle);

            //turn camera
            if (Mouse.LeftButton == true){
            _camAngle += 0.01f* Mouse.Velocity.x*DeltaTime*-1; 
            }
            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered farame) on the front buffer.
            Present();
        }


        // Is called when the window was resized
        public override void Resize()
        {
            // Set the new rendering area to the entire new windows size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width / (float)Height;

            // 0.25*PI Rad -> 45° Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }
    }
}