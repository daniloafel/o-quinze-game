using UnityEngine;
using System.Collections;
//List Usage
using System.Collections.Generic;

/*	GeometricVisibility
	
Steps:
	1.	get all polygons
	1.1.	
	1.2.	

*/

public class GeometricVisibility : MonoBehaviour {
	
	private	Camera cam;			//camera, for panning zooming etc.
	public	Transform source;	//the Transform of the lightSource, for position setting/getting
	
	//shadow caster GameObjects
	GameObject[] walls;
	
	//polygons as struct allowing to have objects references
	private BottomPolygon[]	staticBlockers; //immoveable walls
	//private BottomPolygon[]	dynamicBlockers; //dynamic blockers, recalc positions for relevance
			
	private struct BottomPolygon{
		//vertices of this polygon, Vector2 would be ok, but i aim to create a visibility prism out of the 2D texture
		public Vector3[] vertices;
		
		//triangles of this polygon
		public int[] triangles;
		
		//the center position
		public Vector3 position;
		
		//radius around center where this polygon has to be taken into calculation
		public float relevantRadius;
		
		//the corresponding fader that fades out currently invisible walls... future
		public VisibilityFader fader;
		
		//corresponding Transform to recalc vertices in WorldCoordinates if needed
		public Transform transform;
	}
	
	#region Startup & Update
	public Material wallMaterial;
	private	void Awake(){
		cam = transform.GetComponent<Camera>();
	}
	
	private	void Start(){
		walls = GameObject.FindGameObjectsWithTag("Wall");
		
		//Struct
		GeneratePolygonStructArr(ref staticBlockers);
		
		//Array
		//CreatePolygonsArray();
	}
	
	private	void Update () {
		
		CameraManipulation();
				
		//Struct
		if(drawLineToVertices)	{ DrawLineToVertices(ref staticBlockers);}
		if(drawPolygons)		{ DrawPolygons(ref staticBlockers);}
		if(drawVisibleFaces)	{ DrawVisibleFaces(ref staticBlockers);}
		if(drawSegments)		{ }
		if(drawLines)			{ }
		if(drawCuts)			{ }
		if(drawRecalc)			{ }
		if(drawFinal)			{ }
				
		if(fadeTest)			{FaderTest(ref staticBlockers);}
		
		//Array OLD
		//DrawLineToVerticesOLD();
		//DrawPolygonsOLD();
		//DrawVisibleFacesOLD();
	}
	#endregion
	
	#region DRAWLINE
	
	//just to see the order of the vertices, different colors to see order
	private	Color[] colors = new Color[] {new Color(0F,0F,1F,1F), new Color(0F,0F,1F,0.6F), new Color(0F,0F,1F,0.4F), new Color(0F,0F,1F,0.2F), };
	private	void DrawLineToVertices(ref BottomPolygon[] poly){
		for(int ip = 0; ip < poly.Length; ip++){	//polygon
			for(int iv = 0; iv < poly[ip].vertices.Length; iv++){	//vertices of the polygon
				Color color = new Color(0F,0.5F,1F,1F);
				if(iv != 0){color = colors[iv%4];}
				Debug.DrawLine(source.position, poly[ip].vertices[iv], color, 0F, false);
			}
		}
	}
	
	//draw the polygon CYAN
	private	void DrawPolygons(ref BottomPolygon[] poly){
		for(int ip = 0; ip < poly.Length; ip++){	//polygon
			for(int iv = 0; iv < poly[ip].vertices.Length; iv++){	//vertices of the polygon
				int nv = (iv+1)%poly[ip].vertices.Length; //next vertex
				Debug.DrawLine(poly[ip].vertices[iv], poly[ip].vertices[nv], new Color(0F,1F,1F,0.4F), 0F, false);
			}
		}
	}
	
	private	void DrawVisibleFaces(ref BottomPolygon[] poly){
		for(int ip = 0; ip < poly.Length; ip++){	//polygon
			for(int iv = 0; iv < poly[ip].vertices.Length; iv++){	//vertices of the polygon
				int nv = (iv+1)%poly[ip].vertices.Length; //next vertex
				
				Vector3 vertexDirection = poly[ip].vertices[iv]-poly[ip].vertices[nv];
				Vector3 sourceDirection = source.position-poly[ip].vertices[iv];
				if( AngleDir(vertexDirection,sourceDirection,Vector3.up)<0F ){
					Debug.DrawLine(poly[ip].vertices[iv], poly[ip].vertices[nv], Color.white,0F,false);
				}
			}
		}
	}
	
	//check if a point is left or right to a direction vector, can be reduced for 2 only
	private float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up) {
		Vector3 perp = Vector3.Cross(fwd, targetDir);
		float dir	 = Vector3.Dot(perp, up);
		if		(dir > 0F)	{ return  1F;}//RIGHT
		else if	(dir < 0F)	{ return -1F;}//LEFT
		else				{ return  0F;}
	}
	#endregion
		
	#region STRUCT polygon creation
	private	void GeneratePolygonStructArr(ref BottomPolygon[] poly){
		poly = new BottomPolygon[walls.Length];
		
		int iPoly = 0;	//polygon integrator
		foreach(GameObject wall in walls){
			Mesh mesh = wall.GetComponent<MeshFilter>().mesh;
			Vector3[] vertices = mesh.vertices;
			int[] triangles = mesh.triangles;
		
		//SIZECHECK, list usage could get rid of this step
			//check how much valid vertex are present to assign array lengths in the next step
			int validVertices = 0;
			for(int i = 0; i < vertices.Length; i++){
				//BOTTOM, or which orthogonal direction you need... e.g. sidescroller: if mesh.normals.z-1
				if(mesh.normals[i].y == -1){	//if the normal of this vertice is pointing down, should be only 4 vertices per rectangle
					validVertices++;
				}
			}
									
		//BOTTOM-VERTICES of the walls bottom-poly
			poly[iPoly].vertices = new Vector3[validVertices];	//init new Vector3 array of the struct with the needed length
			int[] validIndices = new int[validVertices];					//the original indices of the valid vertices, used to find the right triangles
			//int[] newIndices = new int[validVertices];						//new indices of the vertices, used to map newTriangles
			
			
			//save the valid vertices and triangles of the current wall
			int iv = 0;	//array integrator
			for(int i = 0; i < vertices.Length; i++){	//for ALL vertices of the wall mesh
				if(mesh.normals[i].y == -1){	//if the normal of this vertice is pointing down, e.g. should be only 4 vertices per rectangle
					//actual saving of the vertex in WORLD COORDINATES
					poly[iPoly].vertices[iv] = wall.transform.TransformPoint(vertices[i]);
					validIndices[iv] = i;
					//newIndices[iv] = iv;
					iv++;
				}
			}
			if(validIndices.Length == 0){break;}//early out
		
		//BOTTOM-TRIANGLES of one poly, maybe we dont need them directly later, but here they are needed to delete inner vertices (e.g. center of cylinder vertex)
			List<int> bottomTriangles = new List<int>();	//using the OLD indices
			int iAs = 0; //iterator for assigned triangles
			for(int it = 0; it < triangles.Length;){// iterator triangles
				//check if the next 3 indices of triangles match
				int match = 0;//check the next 3 indices of this triangles
				for(int imatch = 0; imatch<3; imatch++){
					for(int ivv = 0; ivv < validIndices.Length; ivv++){//check with all vertices
						if(validIndices[ivv]==triangles[it+imatch]){
							match++;
						}
					}
				}
				//if all 3 indices of a triangle match with the validIndices, it is a bottom triangle
				if(match == 3){ //create new triangle in list
					bottomTriangles.Add(triangles[it+0]);
					bottomTriangles.Add(triangles[it+1]);
					bottomTriangles.Add(triangles[it+2]);
					iAs += 3; //assign iterator rdy for next triangle
				}
				it+=3;//next triangle
			}
			//now we have all triangles that are contained in the bottom plane, but with the original indices
						
			int[] bottomTrianglesArr = bottomTriangles.ToArray();
			int[] newTriangles = new int[bottomTrianglesArr.Length];	//using the OLD indices
			//Update indices to refer to poly[iPoly].vertices[iv]:
			for(int ib = 0; ib < bottomTrianglesArr.Length; ib++){
				for(int ivi = 0; ivi < validIndices.Length; ivi++){	//check for original index, assign corresponding new index, must hit once per loop!
					if(bottomTrianglesArr[ib] == validIndices[ivi]){
						newTriangles[ib] = ivi;//currently the same as newTriangles[ib] = newIndices[ivi];, we dont need newIndices[]
					}
				}
			}
			poly[iPoly].triangles = newTriangles;	//currently only used for deleting inner vertices
			
//AT THIS POINT we have the bottom vertices and triangles rdy for any use			
//→(poly[iPoly].vertices,poly[iPoly].triangles);
			
			//now get rid of the inner Vertices:				
			poly[iPoly].vertices = DeleteInnerVertices(poly[iPoly].vertices,poly[iPoly].triangles);
			
			//VERY DIRTY FIX for Vertex order, find dynamic approach
			if(poly[iPoly].vertices.Length == 4)
				RectVertexSwap(ref poly[iPoly].vertices);
			else{
				CylinderVertexSwap(ref poly[iPoly].vertices);
			}
			
						//OTHER assignments for future purpose
							//add and save visibilityFader Reference and set Blackness of the Fader
							if(!wall.GetComponent<VisibilityFader>()){
								poly[iPoly].fader = wall.AddComponent<VisibilityFader>();
								poly[iPoly].fader.ManualInit();
							}else{
								poly[iPoly].fader = wall.GetComponent<VisibilityFader>();
							}
							poly[iPoly].fader.SetBlackness(0.5F);
							
							//Save Transform reference
							poly[iPoly].transform = wall.transform;
			
							//Not implemented yet
							//poly[iPoly].position = CalculateCenter(poly[iPoly].vertices);
							//poly[iPoly].relevantRadius = CalculateRadius(poly[iPoly].vertices);
						//OTHER END

			iPoly++;
		}
	}
	
	//GeneratePolygonStructArr-helper, deletes inner vertices, returns new vertices, no triangles because it would make no sense without inner vertices
	private Vector3[] DeleteInnerVertices(Vector3[] vertices, int[] triangles){
		List<Vector3> outerVertices = new List<Vector3>();
		for(int iv = 0; iv<vertices.Length; iv++){//for all vertices
			int matches = 0;
			for(int it = 0; it < triangles.Length; it++){
				if(triangles[it] == iv){//check how often this vertex-index appears in the triangles
					matches++;
				}
			}
			//Assumption:
			if(matches < 3){//if vertex is used in less than 3 triangles
				//then this is an outer vertex, add it to list
				outerVertices.Add(vertices[iv]);
			}
		}
		//create array
		return outerVertices.ToArray();
	}
		
	private	void RectVertexSwap(ref Vector3[] polygon){
		Vector3 lastVertex = polygon[polygon.Length-1];
		polygon[polygon.Length-1] = polygon[0];
		polygon[0] = lastVertex;
	}
	
	private	void CylinderVertexSwap(ref Vector3[] polygon){
		Vector3 lastVertex = polygon[0];
		polygon[0] = polygon[1];
		polygon[1] = lastVertex;
	}
	
	//mybe needed... http://stackoverflow.com/questions/5271583/center-of-gravity-of-a-polygon
	private	Vector3 CalculateCenter(ref Vector3[] polygon){
		return Vector3.zero;
	}
	
	//smallest circle problem
	private	float CalculateRadius(ref Vector3[] polygon){
		return 0F;
	}
	
	private	void CreateRandomPolygons(){
		//just a completely random poly for now, no special algortithm like http://www.geometrylab.de/applet-29-en#applet
		int verticesCount = Random.Range(3,6);
		
		Vector3[] vertices = new Vector3[verticesCount];
		GameObject randomPoly = new GameObject();
		
		MeshFilter randMesh = randomPoly.AddComponent<MeshFilter>();
		randMesh.mesh = new Mesh();
		MeshRenderer randRend = randomPoly.AddComponent<MeshRenderer>();
		
		for(int i = 0; i < vertices.Length; i++){
			float x = Random.Range(-20F,20F);
			float y = Random.Range(-20F,20F);
			float z = Random.Range(-20F,20F);
			vertices[i] = new Vector3(x,y,z);
		}
				
		// build mesh, just copied from mgears visibility Mesh Generator
		Vector2[] uvs		  = new Vector2[vertices.Length+1];
		Vector3[] newvertices = new Vector3[vertices.Length+1];
		for (int n = 0; n<newvertices.Length-1; n++){
			newvertices[n] = new Vector3(vertices[n].x, 0, vertices[n].y);
		}
		int[] triangles = new int[newvertices.Length*3];
			
		// triangle list
		int iTL = -1;
		for (int n=0;n<triangles.Length-3;n+=3)
		{
			iTL++;
			triangles[n+2] = newvertices.Length-1;
			if (iTL>=newvertices.Length)
			{
				triangles[n+1] = 0;
				//print ("hit:"+i);
			}else{
				triangles[n+1] = iTL+1;
			}
			triangles[n] = iTL;
		}	
		iTL++;
		// central point
		newvertices[newvertices.Length-1] = new Vector3(0,0,0);
		triangles[triangles.Length-1] = newvertices.Length-1;
		triangles[triangles.Length-2] = 0;
		triangles[triangles.Length-3] = iTL-1;
		
		// Create the mesh
		//Mesh msh = new Mesh();
		randMesh.mesh.vertices = newvertices;
		randMesh.mesh.triangles = triangles;
		randMesh.mesh.uv = uvs;
		Vector3[] downNormals = new Vector3[randMesh.mesh.vertices.Length];
		for(int i = 0; i < randMesh.mesh.vertices.Length; i++){
			downNormals[i] = new Vector3(0F, -1F, 0F);
		}
		randMesh.mesh.normals = downNormals;
		randMesh.mesh.RecalculateBounds();
		randomPoly.tag = "Wall";
		
		//random position
		float xp = Random.Range(-100F,100F);
		//float yp = Random.Range(-100F,100F);
		float zp = Random.Range(-100F,100F);
		randomPoly.transform.position = new Vector3(xp, 0.1F, zp);
		
		walls = GameObject.FindGameObjectsWithTag("Wall");
		GeneratePolygonStructArr(ref staticBlockers);
		
		//set material :D
		randRend.material = wallMaterial;				
	}
	#endregion
	
	
	#region Utility
	//camera panning variables
	private	Vector3 curOrigin;
	private	Vector3 camOrigin;
	private	Vector3 lastPos;
	private	void CameraManipulation(){
		if(!mouseOver){
			RaycastHit hit;
			Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition),out hit);
			Vector3 pos = hit.point;
				if(Input.GetKeyDown("mouse 1")){lastPos = pos;}
				if(Input.GetKey("mouse 1")){
					cam.transform.position = (camOrigin + (curOrigin - lastPos));
					//renew point
					Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition),out hit);
					lastPos		= hit.point;
					camOrigin	= cam.transform.position;
				}else{
					camOrigin	= cam.transform.position;
					curOrigin	= pos;
					if(Input.GetKey("mouse 0")){
						source.position = pos;
					}
				}
			source.position = new Vector3(source.position.x,0F,source.position.z);
		}
		
		if (Input.GetAxis("Mouse ScrollWheel") < 0){	// back, zoom out
			if(cam.orthographicSize * 1.25F < 500F){
				cam.orthographicSize *= 1.25F;
			}
		}else if (Input.GetAxis("Mouse ScrollWheel") > 0){	// forward, zoom in
			if(cam.orthographicSize * 1.25F > 12F){
				cam.orthographicSize *= 0.8F;
			}
		}
	}
	
	private	void FaderTest(ref BottomPolygon[] poly){
		for(int ip = 0; ip < poly.Length; ip++){	//polygon
			poly[ip].fader.FadeIn();
		}
	}
	
	//GUI Utility
	private	bool drawLineToVertices	= true;
	private	bool drawPolygons		= true;
	private	bool drawVisibleFaces	= true;
	private	bool drawSegments		= false;
	private	bool drawLines			= false;
	private	bool drawCuts			= false;
	private	bool drawRecalc			= false;
	private	bool drawFinal			= false;
	
	private	bool fadeTest			= false;
	
	public	string lastTooltip = " ";
	private	bool mouseOver = false;
	
	private	void OnGUI(){
		//Quick GUI, just copied it from somthing and inserted the right bools
		int	bh	= 22;
		int	y	= 10;
		int	dy	= 22;
		int	bl	= 120;
		GUI.skin.button.fontSize = 11;
		GUI.skin.toggle.fontSize = 11;
		GUI.skin.label .fontSize = 12;
		
	//Controls
		GUI.skin.label .fontSize = 22;
		GUI.Label (new Rect(Screen.width-230,y,230,bh*4),"Controls");
		GUI.skin.label .fontSize = 12;
		GUI.Label (new Rect(Screen.width-225,y*4,230,bh*4),"" +
			"Light Position:\tLeft Mouse\n" +
			"Zoom:\tMouseWheel\n"+
			"Pan:\t\tHold RightMouseButton\n\n"+
			"ENABLE GIZMOS!");
		
		GUI.Label (new Rect(10,y,600,bh),"Create Random Polygon (just a test if i am able to create anything renderable by script...):"); y+=dy;
		if(GUI.Button (new Rect(10,	y, bl, bh),	new GUIContent("Create","0")))	{CreateRandomPolygons();}y+=dy;
		
		GUI.Label (new Rect(10,y,400,bh),"Draw:"); y+=dy;
		if(GUI.Button (new Rect(10,	y, bl, bh),	new GUIContent("LineToVertices","1")))	{drawLineToVertices = !drawLineToVertices;}y+=dy;
		if(GUI.Button (new Rect(10,	y, bl, bh),	new GUIContent("Polygons",		"2")))	{drawPolygons = !drawPolygons;}y+=dy;
		if(GUI.Button (new Rect(10,	y, bl, bh),	new GUIContent("VisibleFaces",	"3")))	{drawVisibleFaces = !drawVisibleFaces;}y+=dy;
		GUI.enabled = false;
		if(GUI.Button (new Rect(10,	y, bl, bh),	new GUIContent("Segments",		"4")))	{drawSegments = !drawSegments;}y+=dy;
		if(GUI.Button (new Rect(10,	y, bl, bh),	new GUIContent("Lines",			"5")))	{drawLines = !drawLines;}y+=dy;
		if(GUI.Button (new Rect(10,	y, bl, bh),	new GUIContent("Cuts",			"6")))	{drawCuts = !drawCuts;}y+=dy;
		if(GUI.Button (new Rect(10,	y, bl, bh),	new GUIContent("Recalc",		"7")))	{drawRecalc = !drawRecalc;}y+=dy;
		if(GUI.Button (new Rect(10,	y, bl, bh),	new GUIContent("Final",			"8")))	{drawFinal = !drawFinal;}y+=dy;
		GUI.enabled = true;
		
		//indicators
		y-=dy*8;
		GUI.enabled = false;int off = 135;
		drawLineToVertices	= (GUI.Toggle (new Rect(off,	y, 70, bh),	drawLineToVertices, ""));	y+=dy;
		drawPolygons		= (GUI.Toggle (new Rect(off,	y, 70, bh),	drawPolygons, ""));			y+=dy;
		drawVisibleFaces	= (GUI.Toggle (new Rect(off,	y, 70, bh),	drawVisibleFaces, ""));		y+=dy;
		drawSegments		= (GUI.Toggle (new Rect(off,	y, 70, bh),	drawSegments, ""));			y+=dy;
		drawLines			= (GUI.Toggle (new Rect(off,	y, 70, bh),	drawLines, ""));			y+=dy;
		drawCuts			= (GUI.Toggle (new Rect(off,	y, 70, bh),	drawCuts, ""));				y+=dy;
		drawRecalc			= (GUI.Toggle (new Rect(off,	y, 70, bh),	drawRecalc, ""));			y+=dy;
		drawFinal			= (GUI.Toggle (new Rect(off,	y, 70, bh),	drawFinal, ""));			y+=dy;
		GUI.enabled = true;
						
		y+=dy;
		if(GUI.Button (new Rect(10,	y, bl, bh),	new GUIContent("TestFader",		"9")))	{fadeTest = !fadeTest;}y+=dy;
				
		//Mouse above GUI check
		if (Event.current.type == EventType.Repaint && GUI.tooltip != lastTooltip) {
			if (lastTooltip != "")
				SendMessage("OnMouseOut", SendMessageOptions.DontRequireReceiver);
			
			if (GUI.tooltip != "")
				SendMessage("OnMouseOver", SendMessageOptions.DontRequireReceiver);
			lastTooltip = GUI.tooltip;
		}
		//↘↓↙←↖↑↗→
	}
	
	private	void OnMouseOver()	{ mouseOver	=  true;}
	private	void OnMouseOut()	{ mouseOver	= false;}
	
	#endregion
	
		
	#region ARRAY polygon creation && OLD functions
	//OLD Vector[][]: geometric data, the polygon vertices that are on the bottom face [polygon#][vertex#]
	
	//polygons as jagged array
	Vector3[][]	polygons;
	
	//saves all polygon vertices, if all are static we only need to do this at startup, keep seperate list for moveable sightblockers maybe...
	private	void CreatePolygonsArray(){
		polygons = new Vector3[walls.Length][];
		int iPoly= 0;
		foreach(GameObject wall in walls){
			Mesh mesh = wall.GetComponent<MeshFilter>().mesh;
			Vector3[] vertices = mesh.vertices;
			
			//check how much valid vertex are present
			int validVertices = 0;
			for(int i = 0; i < vertices.Length; i++){
				if(mesh.normals[i].y == -1){	//if the normal of this vertice is pointing down, should be only 4 vertices per rectangle
					validVertices++;
				}
			}
			
			//init new Vector3 array with the needed length
			polygons[iPoly] = new Vector3[validVertices];
			
			//save the vertices of the current wall
			int iv = 0;	//array integrator
			for(int i = 0; i < vertices.Length; i++){
				if(mesh.normals[i].y == -1){	//if the normal of this vertice is pointing down, should be only 4 vertices per rectangle
					polygons[iPoly][iv] = wall.transform.TransformPoint(vertices[i]);	//actual saving of the vertex in WORLD COORDINATES
					iv++;
				}
			}
			
			//DIRTY FIX won't do the trick for e.g. cylinder but for rectangle it works for now
			if(validVertices == 4){
				RectVertexSwap(ref polygons[iPoly]);
			}
			
			iPoly++;
			//*LIST usage could do the above in one loop and easier, but i wanted it as an array for now
			// also saving as 2D Vectors would be better
		}
	}
	
	//draw the polygon CYAN
	private	void DrawPolygonsOLD(){
		for(int ip = 0; ip < polygons.Length; ip++){	//polygon
			for(int iv = 0; iv < polygons[ip].Length; iv++){	//vertices of the polygon
				int nv = (iv+1)%polygons[ip].Length; //next vertex
				Debug.DrawLine(polygons[ip][iv], polygons[ip][nv], new Color(0F,1F,1F,0.5F), 0F, false);
			}
		}
	}
	
	//just to see the order of the vertices, different colors to see order
	private	void DrawLineToVerticesOLD(){
		for(int ip = 0; ip < polygons.Length; ip++){	//polygon
			for(int iv = 0; iv < polygons[ip].Length; iv++){	//vertices of the polygon
				Color color = new Color(0F,0.5F,1F,1F);
				if(iv != 0){color = colors[iv%4];}
				Debug.DrawLine(source.position, polygons[ip][iv], color, 0F, false);
			}
		}
	}
	
	//draw the Visible Faces of each polygon GREEN
	private	void DrawVisibleFacesOLD(){
		for(int ip = 0; ip < polygons.Length; ip++){	//polygon
			for(int iv = 0; iv < polygons[ip].Length; iv++){	//vertices of the polygon
				int nv = (iv+1)%polygons[ip].Length; //next vertex
				
				Vector3 vertexDirection = polygons[ip][iv]-polygons[ip][nv];
				Vector3 sourceDirection = source.position-polygons[ip][iv];
				if( AngleDir(vertexDirection,sourceDirection,Vector3.up)<0F ){
					Debug.DrawLine(polygons[ip][iv], polygons[ip][nv], Color.white,0F,false);
				}
			}
		}
		
		//Testing this
		/*
		Debug.DrawLine(polygons[1][0], polygons[1][1], Color.yellow,0F,false);
		Vector3 vertexDirection = polygons[1][0]-polygons[1][1];
		Vector3 sourceDirection = source.position-polygons[1][0];
		Debug.DrawLine(Vector3.zero, vertexDirection, Color.green,0F,false);
		Debug.DrawLine(Vector3.zero, sourceDirection, Color.red,0F,false);
		if( AngleDir(vertexDirection,sourceDirection,Vector3.up)>0F ){
			Debug.DrawLine(polygons[1][0], polygons[1][1], Color.red,0F,false);
		}
		*/
	}	
	#endregion
}
