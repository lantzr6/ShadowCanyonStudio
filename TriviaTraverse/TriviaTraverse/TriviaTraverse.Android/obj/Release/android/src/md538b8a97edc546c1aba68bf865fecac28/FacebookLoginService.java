package md538b8a97edc546c1aba68bf865fecac28;


public class FacebookLoginService
	extends md5b60ffeb829f638581ab2bb9b1a7f4f3f.FormsApplicationActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("TriviaTraverse.Facebook.Droid.Services.FacebookLoginService, TriviaTraverse.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", FacebookLoginService.class, __md_methods);
	}


	public FacebookLoginService () throws java.lang.Throwable
	{
		super ();
		if (getClass () == FacebookLoginService.class)
			mono.android.TypeManager.Activate ("TriviaTraverse.Facebook.Droid.Services.FacebookLoginService, TriviaTraverse.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

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
