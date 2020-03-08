using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Unity.Collections;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Compilation;
using UnityEngine;
using Wayn.Mgm.Effects;

public class EffectBufferGenerator {

    [DidReloadScripts]
    static void OnProjectLoadedInEditor()
    {
        GenerateCode();
    }

    CodeCompileUnit targetUnit;
    CodeTypeDeclaration EffectQueueReferencesClass;
    CodeTypeDeclaration EffectRegistryClass;
    CodeTypeDeclaration EffectBufferClass;

    public EffectBufferGenerator()
    {
        targetUnit = new CodeCompileUnit();
    }

    public void AddNamespace()
    {
        CodeNamespace samples = new CodeNamespace("Wayn.Mgm.Effects.Generated");

        EffectQueueReferencesClass = new CodeTypeDeclaration("EffectQueueReferences");
        EffectQueueReferencesClass.IsStruct = true;
        EffectQueueReferencesClass.TypeAttributes = TypeAttributes.Public;

        EffectRegistryClass = new CodeTypeDeclaration("EffectRegistry");
        EffectRegistryClass.IsStruct = true;
        EffectRegistryClass.TypeAttributes = TypeAttributes.Public;

        EffectBufferClass = new CodeTypeDeclaration("EffectBuffer");
        EffectBufferClass.IsStruct = true;
        EffectBufferClass.TypeAttributes = TypeAttributes.Public;

        samples.Types.Add(EffectQueueReferencesClass);
        samples.Types.Add(EffectRegistryClass);
        samples.Types.Add(EffectBufferClass);

        targetUnit.Namespaces.Add(samples);
    }

    public void BuildClasses()
    {
        Type effectTypeInterface = typeof(IEffect);
        IEnumerator<Type> effectTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => effectTypeInterface.IsAssignableFrom(p)).GetEnumerator();

        Type consumerSystemTypeInterface = typeof(ConsumerSystem);

        IEnumerator<Type> consumerSystemTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => p.IsClass && !p.IsAbstract && p.IsSubclassOf(consumerSystemTypeInterface)).GetEnumerator();

        CodeConstructor constructorEffectQueueReferencesClass = new CodeConstructor();
        constructorEffectQueueReferencesClass.Attributes = MemberAttributes.Public;
        constructorEffectQueueReferencesClass.Parameters.Add(new CodeParameterDeclarationExpression("Unity.Entities.World", "world"));

        CodeConstructor constructorEffectBufferClass = new CodeConstructor();
        constructorEffectBufferClass.Attributes = MemberAttributes.Public;
        constructorEffectBufferClass.Parameters.Add(new CodeParameterDeclarationExpression("Unity.Entities.World", "world"));
        constructorEffectBufferClass.Statements.Add(new CodeSnippetExpression("effectQueueReferences = new EffectQueueReferences(world)"));
        constructorEffectBufferClass.Statements.Add(new CodeSnippetExpression("effectRegistry = new EffectRegistry(1)"));
        EffectBufferClass.Members.Add(constructorEffectBufferClass);

        CodeMemberField effectQueueReferences = new CodeMemberField();
        effectQueueReferences.Attributes = MemberAttributes.Private;
        effectQueueReferences.Name = "effectQueueReferences";
        effectQueueReferences.Type = new CodeTypeReference("EffectQueueReferences");
        EffectBufferClass.Members.Add(effectQueueReferences);

        CodeMemberField effectRegistry = new CodeMemberField();
        effectRegistry.Attributes = MemberAttributes.Public | MemberAttributes.Final;
        effectRegistry.Name = "effectRegistry";
        effectRegistry.Type = new CodeTypeReference("EffectRegistry");
        EffectBufferClass.Members.Add(effectRegistry);


        CodeConstructor constructorEffectRegistryClass = new CodeConstructor();
        constructorEffectRegistryClass.Attributes = MemberAttributes.Public;
        constructorEffectRegistryClass.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(int)), "defaultAllocationSize"));


        CodeMemberMethod AddMethod = new CodeMemberMethod();
        AddMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
        AddMethod.ReturnType = new CodeTypeReference(typeof(void));
        AddMethod.Name = "Add";
        AddMethod.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(ulong)), "effectTypeId"));
        AddMethod.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(int)), "effectVersionId"));
        AddMethod.Parameters.Add(new CodeParameterDeclarationExpression("Unity.Entities.Entity", "other"));
        AddMethod.Parameters.Add(new CodeParameterDeclarationExpression("Unity.Entities.Entity", "emmiter"));

        StringBuilder switchStatement = new StringBuilder();
        switchStatement.AppendLine("switch(effectTypeId)");
        switchStatement.AppendLine("            {");



        CodeMemberMethod AddEffectVerion = new CodeMemberMethod();
        AddEffectVerion.Attributes = MemberAttributes.Public | MemberAttributes.Final;
        AddEffectVerion.ReturnType = new CodeTypeReference(typeof(int));
        AddEffectVerion.Name = "AddEffectVerion";
        AddEffectVerion.Parameters.Add(new CodeParameterDeclarationExpression(typeof(IEffect), "effect"));


        StringBuilder switchStatementAddEffectVerion = new StringBuilder();
        switchStatementAddEffectVerion.AppendLine("switch(effect)");
        switchStatementAddEffectVerion.AppendLine("            {");

        while (effectTypes.MoveNext())
        {           
            Type effectType = effectTypes.Current;
            if (!effectType.IsValueType) continue;

            CodeMemberField nativeQueue = new CodeMemberField();
            nativeQueue.Attributes = MemberAttributes.Private;
            nativeQueue.Name = $"writer{effectType.Name}";
            nativeQueue.Type = new CodeTypeReference($"Unity.Collections.NativeQueue<{effectType.FullName}>.ParallelWriter");
            EffectQueueReferencesClass.Members.Add(nativeQueue);

            constructorEffectQueueReferencesClass.Statements.Add(new CodeSnippetExpression($"{nativeQueue.Name} = world.GetOrCreateSystem<{FindSystemTypeFullName(effectType, consumerSystemTypes)}>().GetConsumerQueue()"));

            CodeMemberMethod EnqueMethod = new CodeMemberMethod();
            EnqueMethod.Attributes= MemberAttributes.Public | MemberAttributes.Final; 
            EnqueMethod.ReturnType = new CodeTypeReference(typeof(void));
            EnqueMethod.Name = "Enqueue";
            EnqueMethod.Parameters.Add(new CodeParameterDeclarationExpression(effectType.FullName, "effect"));
            EnqueMethod.Parameters.Add(new CodeParameterDeclarationExpression("Unity.Entities.Entity", "other"));
            EnqueMethod.Parameters.Add(new CodeParameterDeclarationExpression("Unity.Entities.Entity", "emmiter"));
            EnqueMethod.Statements.Add(new CodeSnippetExpression("effect.Emmiter = emmiter "));
            EnqueMethod.Statements.Add(new CodeSnippetExpression("effect.Other = other "));
            EnqueMethod.Statements.Add(new CodeSnippetExpression($"{nativeQueue.Name}.Enqueue(effect) "));
            EffectQueueReferencesClass.Members.Add(EnqueMethod);

            CodeMemberField nativeList = new CodeMemberField();
            nativeList.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            nativeList.Name = $"nativeListOf{effectType.Name}";
            nativeList.Type = new CodeTypeReference($"Unity.Collections.NativeList<{effectType.FullName}>");
            EffectRegistryClass.Members.Add(nativeList);
            constructorEffectRegistryClass.Statements.Add(new CodeSnippetExpression($"{nativeList.Name} = new Unity.Collections.NativeList<{effectType.FullName}>(defaultAllocationSize,Unity.Collections.Allocator.Persistent)"));
            
            switchStatement.AppendLine($"               case {EffectReference.GetTypeId(effectType)}:");
            switchStatement.AppendLine($"                   effectQueueReferences.Enqueue(effectRegistry.{nativeList.Name}[effectVersionId], other, emmiter);");
            switchStatement.AppendLine("                    break;");


            switchStatementAddEffectVerion.AppendLine($"               case {effectType.FullName} e:");
            switchStatementAddEffectVerion.AppendLine($"                   { nativeList.Name}.Add(e);");
            switchStatementAddEffectVerion.AppendLine($"                   return {nativeList.Name}.Length - 1;");


           

        }

        switchStatement.AppendLine("            }");
        AddMethod.Statements.Add(new CodeSnippetExpression(switchStatement.ToString()));


        switchStatementAddEffectVerion.AppendLine("            }");
        AddEffectVerion.Statements.Add(new CodeSnippetExpression(switchStatementAddEffectVerion.ToString()));
        AddEffectVerion.Statements.Add(new CodeSnippetExpression($"return 1"));

        EffectBufferClass.Members.Add(AddMethod);
        EffectRegistryClass.Members.Add(AddEffectVerion);
        EffectRegistryClass.Members.Add(constructorEffectRegistryClass);
        EffectQueueReferencesClass.Members.Add(constructorEffectQueueReferencesClass);
    }


    /*
     *   
     *  public int AddEffectVerion(IEffect effect)
        {
            switch (effect)
            {
                case Wayn.Mgm.Combat.Effects.ChangeHealthEffect e:
                    nativeListOfChangeHealthEffect.Add(e);
                    return nativeListOfChangeHealthEffect.Length - 1;
            }
            return -1;
        }
     * */

    public string FindSystemTypeFullName(Type effectType, IEnumerator<Type> consumerSystemTypes)
    {
        while (consumerSystemTypes.MoveNext())
        {
            Type consumerSystemType = consumerSystemTypes.Current;
            if (!consumerSystemType.BaseType.GetGenericArguments().Contains(effectType)) continue;
            return consumerSystemType.FullName;
        }
        throw new Exception($"Could not find consumer system for {effectType.FullName}");   
    }

    public bool CompileCSharpCode(string exeFile)
    {

        CSharpCodeProvider provider = new CSharpCodeProvider();

        // Build the parameters for source compilation.
        CompilerParameters cp = new CompilerParameters();



        // Add an assembly reference.

        System.Reflection.Assembly netStandard = System.Reflection.Assembly.Load("netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null");
        cp.ReferencedAssemblies.Add(netStandard.Location);
        cp.ReferencedAssemblies.Add(System.Reflection.Assembly.GetAssembly(typeof(UnityEngine.MonoBehaviour)).Location);
        AddAssemblies(cp, CompilationPipeline.GetAssemblies(AssembliesType.PlayerWithoutTestAssemblies));

        cp.GenerateExecutable = false;

        // Set the assembly file name to generate.
        cp.OutputAssembly = exeFile;

        // Save the assembly as a physical file.
        cp.GenerateInMemory = false;


        // Invoke compilation.


        
        CompilerResults cr = provider.CompileAssemblyFromDom(cp, new CodeCompileUnit[] { targetUnit });
    
        // Return the results of compilation.
        if (cr.Errors.Count > 0)
        {
            var errors = cr.Errors.GetEnumerator();
            while (errors.MoveNext())
            {
                Debug.LogError(errors.Current.ToString());
            };
            return false;
        }
        else
        {
            return true;
        }
    }

    private static void AddAssemblies(CompilerParameters cp, UnityEditor.Compilation.Assembly[] assemblies)
    {
        foreach (UnityEditor.Compilation.Assembly assembly in assemblies)
        {

            if (!cp.ReferencedAssemblies.Contains(assembly.outputPath))
            {
                cp.ReferencedAssemblies.Add(assembly.outputPath);
            }
            AddAssemblies(cp, assembly.assemblyReferences);
        }
    }

    public void GenerateCSharpCode(string fileName)
    {
        

        CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
        CodeGeneratorOptions options = new CodeGeneratorOptions();
        options.BracingStyle = "C";
        using (StreamWriter sourceWriter = new StreamWriter(fileName))
        {
            provider.GenerateCodeFromCompileUnit(
                targetUnit, sourceWriter, options);
        }
    }

    private static void GenerateCode()
    {
        EffectBufferGenerator t = new EffectBufferGenerator();
        t.AddNamespace();
        t.BuildClasses();
        t.GenerateCSharpCode($"{Application.dataPath}\\GeneratedEffectBuffer.cs");
        //t.CompileCSharpCode($"{Application.dataPath}\\Plugins\\Generated.dll");
        
        AssetDatabase.Refresh();
    }
}
 