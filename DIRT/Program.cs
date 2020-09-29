﻿using System;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace DIRT
{
    class Program
    {
        static void Main(string[] args)
        {

            DIRT.startRenderer();
            
            Mesh m = new Mesh(new Vector(0,-1,10,0), Vector.zero);
            m.makeFromOBJ("./teapot.obj");
            //m.rotation.z = Math.PI/90;
            m.rotation.y = Math.PI;

            DIRT.Meshes.Add(m);

            /*
            Mesh m3 = new Mesh(new Vector(2, -3, 2), Vector.zero);
            m3.makeFromOBJ("./teapot.obj");
            m3.rotation.z = Math.PI / 90;
            m3.rotation.y = Math.PI;*/

            //DIRT.Meshes.Add(m3);
            /*
            Random rnd = new Random();

            Mesh[] mesh = new Mesh[10];*/
            /*
            Vector[] r = new Vector[10];

            for (int i = 0; i < mesh.Length; i++)
            {
                double rot = rnd.NextDouble() * Math.PI*2;
                double y = (rnd.NextDouble() -0.5) * 10;
                double dist = (rnd.NextDouble() * 20) + 5;


                mesh[i] = new Mesh(new Vector(Math.Cos(rot)*dist,y, Math.Sin(rot) * dist),Vector.zero);

                if (rnd.Next(0,100) > 50)
                {
                    mesh[i].makePyramid((rnd.NextDouble() * 1) + 0.5);
                }
                else
                {
                    mesh[i].makeCube((rnd.NextDouble() * 1) + 0.5);
                }

                

                DIRT.Meshes.Add(mesh[i]);

                r[i] = new Vector((rnd.NextDouble() - 0.5) / 20, (rnd.NextDouble() - 0.5) / 20, (rnd.NextDouble() - 0.5) / 20);
            }*/
            /*
            for (int i = 0; i < mesh.Length; i++)
            {
                mesh[i] = new Mesh(new Vector(Math.Cos(((Math.PI/mesh.Length)*2)*i) * 5,1, Math.Sin(((Math.PI / mesh.Length) * 2) * i) * 5),Vector.zero);
                mesh[i].makeCube(1);
                DIRT.Meshes.Add(mesh[i]);
            }*/

            /*
            Mesh sphere = new Mesh(new Vector(0,0,3),Vector.zero);

            //sphere.makeFromOBJ("./sphere.obj");

            DIRT.Meshes.Add(sphere);*/
            /*
            Mesh m = new Mesh(new Vector(0,0,2,0),new Vector(0,0,0,0));

            m.makeCube(1);

            DIRT.Meshes.Add(m);*/


            while (true)
            {
                Thread.Sleep(33);

                lock (DIRT.renderLock)
                {
                    
                    //Settings.globalRot.y += 0.002;
                    /*
                    for (int i = 0; i < mesh.Length; i++)
                    {
                        mesh[i].rotation += r[i];
                    }*/

                    //m.rotation.x += 0.01;
                    m.rotation.y += 0.02;
                    //m.rotation.z += 0.03;
                    //m.position.x += 0.01;
                    //m.position = new Vector(Math.Cos(rot)*5,0,Math.Sin(rot)*5);

                    //Settings.cameraRot.y += 0.01;

                }

                switch (Screen.getInput())
                {

                }

            }

            
            



        }
    }

}
