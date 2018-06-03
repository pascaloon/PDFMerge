/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:PDFMerge"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Ninject;
using Ninject.Modules;
using PDFMerge.Services;

namespace PDFMerge.ViewModel
{

    class ViewModelModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<MainViewModel>().ToSelf();
            this.Bind<PDFMergerViewModel>().ToSelf();
            this.Bind<PDFMerger>().ToSelf();
        }
    }


    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        ViewModelModule _module;
        StandardKernel _kernel;

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            _module = new ViewModelModule();
            _kernel = new StandardKernel(_module);

            //ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return _kernel.Get<MainViewModel>();
                //return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public PDFMergerViewModel PDFMergerViewModel => _kernel.Get<PDFMergerViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}