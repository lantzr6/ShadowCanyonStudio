package md557eb9a97d39f5c7eea5115f21730c0e7;


public class GraphCallback
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.facebook.GraphRequest.Callback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCompleted:(Lcom/facebook/GraphResponse;)V:GetOnCompleted_Lcom_facebook_GraphResponse_Handler:Xamarin.Facebook.GraphRequest/ICallbackInvoker, Xamarin.Facebook\n" +
			"";
		mono.android.Runtime.register ("TriviaTraverse.Facebook.Droid.Objects.GraphCallback, TriviaTraverse.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", GraphCallback.class, __md_methods);
	}


	public GraphCallback () throws java.lang.Throwable
	{
		super ();
		if (getClass () == GraphCallback.class)
			mono.android.TypeManager.Activate ("TriviaTraverse.Facebook.Droid.Objects.GraphCallback, TriviaTraverse.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCompleted (com.facebook.GraphResponse p0)
	{
		n_onCompleted (p0);
	}

	private native void n_onCompleted (com.facebook.GraphResponse p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
