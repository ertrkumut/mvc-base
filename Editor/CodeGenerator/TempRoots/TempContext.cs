﻿using MVC.Runtime.Contexts;

namespace MVC.Editor.CodeGenerator.TempRoots
{
    internal class TempContext : Context
    {
        //SCREEN_FLAG
        //TEST_FLAG

        public override void SignalBindings()
        {
            base.SignalBindings();
        }

        public override void InjectionBindings()
        {
            base.InjectionBindings();
        }

        public override void MediationBindings()
        {
            base.MediationBindings();
        }

        public override void CommandBindings()
        {
            base.CommandBindings();
        }

        public override void Setup()
        {
            base.Setup();
        }
        
        public override void Launch()
        {
            base.Launch();
        }
    }
}