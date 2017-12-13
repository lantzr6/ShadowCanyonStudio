package mono.android.app;

public class ApplicationRegistration {

	public static void registerApplications ()
	{
				// Application and Instrumentation ACWs must be registered first.
		mono.android.Runtime.register ("TriviaTraverse.Droid.DeviceStepsImplementation, TriviaTraverse.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", md54dcdbff2a0823650b18ef95cc6d21b89.DeviceStepsImplementation.class, md54dcdbff2a0823650b18ef95cc6d21b89.DeviceStepsImplementation.__md_methods);
		mono.android.Runtime.register ("TriviaTraverse.Droid.GlobalApp, TriviaTraverse.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", md54dcdbff2a0823650b18ef95cc6d21b89.GlobalApp.class, md54dcdbff2a0823650b18ef95cc6d21b89.GlobalApp.__md_methods);
		
	}
}
