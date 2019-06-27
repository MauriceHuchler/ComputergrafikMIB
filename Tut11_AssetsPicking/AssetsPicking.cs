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
    public class AssetsPicking : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRenderer _sceneRenderer;
        private ScenePicker _scenePicker;
        private TransformComponent _baseTransform;
        // Transform vom Panzer
        private TransformComponent _towerTransform;

        private TransformComponent _HRTransform;
        private TransformComponent _HLTransform;

        private TransformComponent _VRTransform;

        private TransformComponent _VLTransform;

        private TransformComponent _RumpfTransform;

        private Boolean canTurn = false;
        private Boolean gasGebenHR =false;
        private Boolean gasGebenHL =false;
        private Boolean allrad = false;

        //
        private PickResult _currentPick;
        private float3 _oldColor;

        

        SceneContainer CreateScene()
        {
            // Initialize transform components that need to be changed inside "RenderAFrame"
            _baseTransform = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 0, 0)
            };

            // Setup the scene graph
            return new SceneContainer
            {
                Children = new List<SceneNodeContainer>
                {
                    new SceneNodeContainer
                    {
                        Components = new List<SceneComponentContainer>
                        {
                            // TRANSFROM COMPONENT
                            _baseTransform,

                            // SHADER EFFECT COMPONENT
                            new ShaderEffectComponent
                            {
                                Effect = SimpleMeshes.MakeShaderEffect(new float3(0.7f, 0.7f, 0.7f), new float3(1, 1, 1), 5)
                            },

                            // MESH COMPONENT
                            // SimpleAssetsPickinges.CreateCuboid(new float3(10, 10, 10))
                            SimpleMeshes.CreateCuboid(new float3(10, 10, 10))
                        }
                    },
                }
            };
        }

        // Init is called on startup. 
        public override void Init()
        {

           
            // Set the clear color for the backbuffer to white (100% intensity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0.8f, 0.9f, 0.7f, 1);

            _scene = AssetStorage.Get<SceneContainer>("panzer.fus");
            _towerTransform = _scene.Children.FindNodes(node => node.Name == "Turm")?.FirstOrDefault()?.GetTransform();
            _HRTransform = _scene.Children.FindNodes(node => node.Name == "Rad-hinten-rechts")?.FirstOrDefault()?.GetTransform();
            _HLTransform = _scene.Children.FindNodes(node => node.Name == "Rad-hinten-links")?.FirstOrDefault()?.GetTransform();
            _VRTransform = _scene.Children.FindNodes(node => node.Name == "Rad-vorne-rechts")?.FirstOrDefault()?.GetTransform();
            _VLTransform = _scene.Children.FindNodes(node => node.Name == "Rad-vorne-Links")?.FirstOrDefault()?.GetTransform();
             _RumpfTransform = _scene.Children.FindNodes(node => node.Name == "Rumpf")?.FirstOrDefault()?.GetTransform();
            

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRenderer(_scene);

            //picker
            _scenePicker = new ScenePicker(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            //_baseTransform.Rotation = new float3(0, M.MinAngle(TimeSinceStart), 0);

            //tower kontrolle
            

           // _towerTransform.Rotation = new float3(0,M.MinAngle(TimeSinceStart),0);
            

            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);



            // Setup the camera 
            RC.View = float4x4.CreateTranslation(0, 0, 20) * float4x4.CreateRotationX(-(float) Atan(15.0 / 40.0));

        if (Mouse.LeftButton)
            {
                float2 pickPosClip = Mouse.Position * new float2(2.0f / Width, -2.0f / Height) + new float2(-1, 1);
                _scenePicker.View = RC.View;
                _scenePicker.Projection = RC.Projection;
                List<PickResult> pickResults = _scenePicker.Pick(pickPosClip).ToList();
                PickResult newPick = null;
                if (pickResults.Count > 0)
                {
                    pickResults.Sort((a, b) => Sign(a.ClipPos.z - b.ClipPos.z));
                    newPick = pickResults[0];
                }

                // Turm nur drehen wenn angeklickt
                 if(pickResults.Count>0 && pickResults[0].Node.Name == "Turm" && _currentPick != null)
                {  
                    canTurn =true;
                }
                else
                {
                    canTurn = false;
                }

                //Räder drehen 
                if(pickResults.Count>0 && _currentPick != null && pickResults[0].Node.Name == "Rad-hinten-rechts")
                {
                    gasGebenHR = true;
                }
                else
                {
                    gasGebenHR = false;
                }

                 if(pickResults.Count>0 && _currentPick != null && pickResults[0].Node.Name == "Rad-hinten-links")
                {
                    gasGebenHL = true;
                }
                else
                {
                    gasGebenHL = false;
                }

                if(pickResults.Count>0 && _currentPick != null && pickResults[0].Node.Name == "Rumpf")
                {
                    allrad = true;
                }
                else
                {
                    allrad = false;
                }

        
                if (newPick?.Node != _currentPick?.Node)
                {
                    if (_currentPick != null)
                    {
                        ShaderEffectComponent shaderEffectComponent = _currentPick.Node.GetComponent<ShaderEffectComponent>();
                        shaderEffectComponent.Effect.SetEffectParam("DiffuseColor", _oldColor);
                    }
                    if (newPick != null)
                    {
                        ShaderEffectComponent shaderEffectComponent = newPick.Node.GetComponent<ShaderEffectComponent>();
                        _oldColor = (float3)shaderEffectComponent.Effect.GetEffectParam("DiffuseColor");
                        shaderEffectComponent.Effect.SetEffectParam("DiffuseColor", new float3(0.5f, 0.1f, 0.4f));
                    }
                    _currentPick = newPick;
                }
            }

           if (canTurn == true)
           {
                  float towerRed = _towerTransform.Rotation.y;
                    towerRed +=0.05f *Keyboard.ADAxis*(-1);
                _towerTransform.Rotation = new float3(0,towerRed,0);
           }

           if(gasGebenHR == true)
           {
               float hintenRechts = _HRTransform.Rotation.x;
               hintenRechts +=0.1f *Keyboard.WSAxis*(-1);
               _HRTransform.Rotation = new float3(hintenRechts,0,0);

           }

            if(gasGebenHL == true)
           {
           
               float hintenLinks =  _HLTransform.Rotation.x;
               hintenLinks +=0.1f *Keyboard.WSAxis*(-1);
               _HLTransform.Rotation = new float3(hintenLinks,0,0);
           }
            // bei Klick auf Rumpf
           if(allrad == true)
           {
               //Hinten rechts
                 float hintenRechts = _HRTransform.Rotation.x;
               hintenRechts +=0.1f *Keyboard.WSAxis*(-1);
               _HRTransform.Rotation = new float3(hintenRechts,0,0);


                // Hinten links
                 float hintenLinks =  _HLTransform.Rotation.x;
               hintenLinks +=0.1f *Keyboard.WSAxis*(-1);
               _HLTransform.Rotation = new float3(hintenLinks,0,0);

               // Vorne rechts
                float vorneRechts =  _VRTransform.Rotation.x;
               vorneRechts +=0.1f *Keyboard.WSAxis*(-1);
               _VRTransform.Rotation = new float3(vorneRechts,0,0);

               //vorne links
                    float vorneLinks =  _VLTransform.Rotation.x;
               vorneLinks +=0.1f *Keyboard.WSAxis*(-1);
               _VLTransform.Rotation = new float3(vorneLinks,0,0);

           }
            
            
               

            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
            Present();
        }


        // Is called when the window was resized
        public override void Resize()
        {
            // Set the new rendering area to the entire new windows size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width / (float)Height;

            // 0.25*PI Rad -> 45� Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }
    }
}
